using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Conversations;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.Player;
using KrazyKrakenGames.DetectiveGame.QuestSystem;
using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    [DefaultExecutionOrder(6)]
    public class ConversationInteraction : InteractableObject
    {
        [Space(15)]
        [Header("Other References")]
        public bool isActive = false;

        [SerializeField] private Transform lookAt;

        [Space(15)]
        [Header("Conversation Related Variables")]

        //Conversation is player's own thoughts?
        [Tooltip("Conversation is player's own thoughts")]
        [SerializeField] private bool isSelfConversation;

        [Space(3)]
        [SerializeField] private ConversationSO conversationSO;
        [SerializeField] private List<ConvoNodeSO> allConversationNodes;
        [SerializeField] private ConvoNodeSO rootNode;
        [SerializeField] private ConvoNodeSO processingParentNode;
        [SerializeField] private Dictionary<ConvoNodeSO, SiblingGroup> allConvoSiblings = new Dictionary<ConvoNodeSO, SiblingGroup>();
        [SerializeField] private SiblingGroup currentGroup;
        [SerializeField] private ConvoNodeSO activeConvoNode;
        private List<ConvoUI> currentOptions = new List<ConvoUI>();
        private ConvoUI selectedOption;
        private int selectedIndex;
        private int lastIndex;
        private float valueChangeThreshold = 0.250f; //Allows the value to be changed after every threshold seconds/frames
        private bool allowChange = true;
        [SerializeField] private bool isLastMessage = false;

        [SerializeField] private NPC_Dialog npc;

        public bool LastMessageShown => isLastMessage;

        [Space(15)]
        [Header("Quest related Variables")]
        [SerializeField] private QuestTrigger questTrigger;
        [SerializeField] private bool IsQuestRunning;   //Running => activated, else => false

        [SerializeField] private bool WaitingForUserResponse = false;
        private void Start()
        {
            if (questTrigger != null)
            {
                if (questTrigger.activeQuest == null)
                {
                    Debug.LogWarning("No quest found");
                    questTrigger.CreateQuest();
                    Debug.Log("Quest created through dialog interaction");
                }

                questTrigger.activeQuest.OnQuestCompleteEvent.AddListener(OnQuestComplete);
                questTrigger.activeQuest.OnQuestStatusChange += OnQuestStatusChangeHandler;
            }

            allConversationNodes = conversationSO.Nodes;

            if(allConversationNodes.Count > 0)
            {
                TraverseConversation();
            }
            else
            {
                Debug.LogWarning($"Conversation has no nodes",gameObject);
            }
         

            if (ActionController.Instance != null)
            {
                ActionController.Instance.OnMoveInputEvent += OnMoveInputHandler;
            }
        }

        private void OnDestroy()
        {
            if (questTrigger != null)
            {
                if (questTrigger.activeQuest != null)
                {
                    questTrigger.activeQuest.OnQuestCompleteEvent.RemoveAllListeners();
                    questTrigger.activeQuest.OnQuestStatusChange -= OnQuestStatusChangeHandler;
                }
            }

            if (ActionController.Instance != null)
            {
                ActionController.Instance.OnMoveInputEvent -= OnMoveInputHandler;
            }
        }

        public override void Interact()
        {
            //Conversation is started from here!
            OnLastMessageShown(false);

            if (!isSelfConversation)
            {
                CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
                
            }

            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);

            ConversationLogic();
        }

        private void ConversationLogic()
        {
            if (questTrigger == null)
            {
                activeConvoNode = rootNode;
            }
            else
            {
                //Check quest's status and then determine from where should the convo start

                var questStatus = questTrigger.activeQuest.Status;

                if(questStatus == QuestStatus.INACTIVE)
                {
                    //No need to jump nodes, we start from top
                    activeConvoNode = rootNode;
                }
                else if (questStatus == QuestStatus.INPROGRESS)
                {
                    //Also need to check which quest segment is completed
                    int index = conversationSO.questInProgressIndex;
                    activeConvoNode = conversationSO.Nodes[index];
                }
                else if(questStatus == QuestStatus.COMPLETED)
                {
                    int index = conversationSO.questCompleteIndex;
                    activeConvoNode = conversationSO.Nodes[index];
                }
            }

            Invoke("StartConversation", 0.8f);
        }

        private void StartConversation()
        {
            isActive = true;

            if (npc != null)
            {
                npc.StartConversation();
            }

            UIManager.instance.ConvoShowDialog(activeConvoNode.messageData.message, npc, this);
            HandlingQuestBasedOnMessageType(activeConvoNode);
        }

        public void PopNextMessage()
        {
            if (selectedOption == null)
            {
                if (activeConvoNode.Children.Count == 0)
                {
                    //Conversation Enders
                    Debug.Log("Leaf node, end conversation");
                    OnLastMessageShown(true);
                }
                else if (activeConvoNode.Children.Count == 1)
                {
                    //One child, flow of conversation
                    activeConvoNode = activeConvoNode.Children[0];
                    UIManager.instance.UpdateDialog(activeConvoNode.messageData.message);
                }
                else if (activeConvoNode.Children.Count > 1)
                {
                    if (WaitingForUserResponse) return;

                    currentOptions.Clear();

                    //Multiple choices present
                    WaitingForUserResponse = true;
                    var siblings = allConversationNodes.Where(item => item.ParentNode == activeConvoNode).ToList();
                    UIManager.instance.UpdateDialog(activeConvoNode.messageData.message);
                    currentOptions = UIManager.instance.ShowOptions(siblings);
                    selectedIndex = 0;
                    lastIndex = currentOptions.Count - 1;
                    selectedOption = currentOptions[selectedIndex];
                    selectedOption.Highlighted();
                }

                HandlingQuestBasedOnMessageType(activeConvoNode);
            }
            else
            {
                //The selected dialog option will be executed
                activeConvoNode = selectedOption.data;
                selectedOption = null;
                WaitingForUserResponse = false;

                foreach (var opt in currentOptions)
                {
                    opt.gameObject.SetActive(false);
                    Destroy(opt.gameObject);
                }

                currentOptions.Clear();

                Invoke("PopNextMessage", valueChangeThreshold);
            }
        }
       

        private void OnLastMessageShown(bool _value)
        {
            isLastMessage = _value;
            UIManager.instance.SetLastMessageStatus(isLastMessage);
        }


        #region Quest Management Handling

        private void HandlingQuestBasedOnMessageType(ConvoNodeSO dialogMessage)
        {
            switch (dialogMessage.messageData.MessageType)
            {
                case DialogMessageType.DEFAULT:
                    break;

                case DialogMessageType.QUESTGIVER:

                    Debug.Log("Starting a new quest from dialog");

                    questTrigger.StartQuest();

                    foreach (var segment in questTrigger.activeQuest.Segments)
                    {
                        if (segment.SegmentID == dialogMessage.messageData.QuestSegmentID)
                        {
                            Debug.Log($"Segment {segment.Title} is completed through dialogue");
                            questTrigger.QuestSegmentComplete(dialogMessage.messageData.QuestSegmentID);
                        }
                    }

                    OnLastMessageShown(true);

                    break;

                case DialogMessageType.QUESTACTIVE:
                    OnLastMessageShown(true);
                    break;

                case DialogMessageType.QUESTENDED:

                    Debug.Log("Active quest segment has ended");
                    OnLastMessageShown(true);
                    break;

                case DialogMessageType.ENDER:
                    Debug.Log("last message of convo shown");
                    OnLastMessageShown(true);
                    break;
            }
        }

        private void OnQuestComplete(Quest completedQuest)
        {

        }

        private void OnQuestStatusChangeHandler(Quest _quest, QuestStatus _newStatus)
        {
            Debug.Log($"Quest status changed to: {_newStatus}");

            //Update active messages to include only said type

            if(_newStatus == QuestStatus.INPROGRESS)
            {
                IsQuestRunning = true;
            }
            else
            {
                IsQuestRunning = false;
            }
        }
        #endregion


        #region Conversation Handling

        public async void TraverseConversation()
        {
            rootNode = allConversationNodes.Where(item => item.ParentNode == null).First();
            processingParentNode = rootNode;

            await SortSiblings();

            Debug.Log("All siblings sorted");

        }

        private async Task SortSiblings()
        {
            var siblingsList = allConversationNodes.Where(item => item.ParentNode == processingParentNode).ToList();

            if (siblingsList.Count > 0)
            {
                SiblingGroup sibGroup = new SiblingGroup(siblingsList, processingParentNode);

                if (!allConvoSiblings.ContainsKey(processingParentNode))
                {
                    allConvoSiblings.Add(processingParentNode, sibGroup);
                }
                else
                {
                    allConvoSiblings[processingParentNode] = sibGroup;
                }

                foreach (var sibling in siblingsList)
                {
                    processingParentNode = sibling;

                    await SortSiblings();
                }
            }
            else
            {
                return;
            }
        }

        #endregion


        #region Input Handling

        private void OnMoveInputHandler(Vector2 _move)
        {
            if (!isActive) return;
            if (!allowChange) return;
            allowChange = false;

            //NOTE: IF OPTION CYCLE SEEMS TO FAIL,DO CHECK THIS LOGIC OUT AGAIN
            if (_move.x > 0)
            {
                SelectPreviousOption();
            }
            else
            {
                SelectNextOption();
            }

            Invoke("AllowInputChange", valueChangeThreshold);
        }

        private void SelectNextOption()
        {
            if(selectedIndex < lastIndex)
            {
                selectedIndex += 1;
            }
            else
            {
                selectedIndex = 0;
            }
            selectedOption = currentOptions[selectedIndex];
            selectedOption.Highlighted();
        }

        private void SelectPreviousOption()
        {
            if (selectedIndex > 0)
            {
                selectedIndex -= 1;
            }
            else
            {
                selectedIndex = lastIndex;
            }
            selectedOption = currentOptions[selectedIndex];
            selectedOption.Highlighted();
           
        }

        private void AllowInputChange()
        {
            allowChange = true;
        }

        public void Reset()
        {
            isLastMessage = false;
            activeConvoNode = rootNode;
        }


        #endregion

    }
}
