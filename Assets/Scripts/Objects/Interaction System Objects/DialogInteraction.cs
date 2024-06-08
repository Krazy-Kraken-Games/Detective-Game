using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.QuestSystem;
using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections.Generic;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    [DefaultExecutionOrder(6)]
    public class DialogInteraction : InteractableObject
    {
        [SerializeField] private Transform lookAt;

        [SerializeField] private List<DialogMessage> allMessages = new List<DialogMessage>();
        [SerializeField] private int currentMessageIndex;
        [SerializeField] private bool isLastMessage = false;
        private DialogMessage currentMessage;

        [SerializeField] private NPC_Dialog npc;

        public bool LastMessageShown => isLastMessage;

        public List<DialogMessage> availableMessages = new List<DialogMessage>();

        [SerializeField] private QuestTrigger questTrigger;
        private void Start()
        {
            if(questTrigger != null)
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

            List<DialogMessageType> allowedType = new List<DialogMessageType>() { DialogMessageType.DEFAULT, DialogMessageType.QUESTGIVER };
            GatherAvailableMessages(allowedType);
        }

        private void OnDestroy()
        {
            if(questTrigger != null)
            {
                if(questTrigger.activeQuest != null)
                {
                    questTrigger.activeQuest.OnQuestCompleteEvent.RemoveAllListeners();
                    questTrigger.activeQuest.OnQuestStatusChange -= OnQuestStatusChangeHandler;
                }
            }
        }

        private void GatherAvailableMessages(List<DialogMessageType> allowedType)
        {
            availableMessages.Clear();
            foreach (var message in allMessages)
            {
                if(allowedType.Contains(message.MessageType))
                {
                    availableMessages.Add(message);
                }
            }
        }

        public override void Interact()
        {
            isLastMessage = false;
            currentMessageIndex = 0;

            currentMessage = availableMessages[currentMessageIndex];

            if (currentMessageIndex == availableMessages.Count - 1)
            {
                OnLastMessageShown();
            }

            HandlingQuestBasedOnMessageType(currentMessage);
            CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);

            Invoke("StartConversation", 0.8f);
        }

        private void StartConversation()
        {
            npc.StartConversation();
           // UIManager.instance.ShowDialog(currentMessage,npc,this);
        }

        public void PopNextMessage()
        {
            currentMessageIndex++;
            currentMessage = availableMessages[currentMessageIndex];

            if(currentMessageIndex == availableMessages.Count - 1)
            {
                OnLastMessageShown();
            }

            HandlingQuestBasedOnMessageType(currentMessage);

           // UIManager.instance.UpdateDialog(currentMessage.Message);
        }

        private void OnLastMessageShown()
        {
            isLastMessage = true;

        }


        #region Quest Management Handling

        private void HandlingQuestBasedOnMessageType(DialogMessage dialogMessage)
        {
            switch(dialogMessage.MessageType)
            {
                case DialogMessageType.DEFAULT:
                    break;

                case DialogMessageType.QUESTGIVER:

                    Debug.Log("Starting a new quest from dialog");

                    questTrigger.StartQuest();

                    foreach (var segment in questTrigger.activeQuest.Segments)
                    {
                        if (segment.SegmentID == dialogMessage.QuestSegmentID)
                        {
                            Debug.Log($"Segment {segment.Title} is completed through dialogue");
                            questTrigger.QuestSegmentComplete(dialogMessage.QuestSegmentID);
                        }
                    }

                    break;

                case DialogMessageType.QUESTACTIVE:

                   break;

                case DialogMessageType.QUESTENDED:

                    Debug.Log("Active quest segment has ended");
                    break;
            }
        }

        private void OnQuestComplete(Quest completedQuest)
        {

        }

        private void OnQuestStatusChangeHandler(Quest _quest,QuestStatus _newStatus)
        {
            Debug.Log($"Quest status changed to: {_newStatus}");

            //Update active messages to include only said type

            if(_newStatus == QuestStatus.INPROGRESS)
            {
                List<DialogMessageType> allowedType = new List<DialogMessageType>() { DialogMessageType.QUESTACTIVE };
                GatherAvailableMessages(allowedType);
            }
            else if(_newStatus == QuestStatus.COMPLETED)
            {
                List<DialogMessageType> allowedType = new List<DialogMessageType>() { DialogMessageType.QUESTENDED };
                GatherAvailableMessages(allowedType);
            }
        }
        #endregion

    }
}
