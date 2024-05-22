using Cinemachine;
using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Conversations;
using KrazyKrakenGames.DetectiveGame.Gameplay;
using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(-1)]
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        [Header("Cross-Hair Reference")]
        public RectTransform crossHair;
        private Vector3 crossHairWorldPos;

        public Vector3 CrossHairWorldPosition => crossHairWorldPos;

        [Space(5)]
        [Header("Dialog System References")]
        [SerializeField] private DialogUISystem dialogSystem;
        [SerializeField] private bool isDialogActive = false;
        [SerializeField] private DialogInteraction activeDialogInteraction;
        [SerializeField] private ConversationInteraction activeConvoInteraction;
        [SerializeField] private bool showingLastMessage = false;
        public bool LastMessageShown => showingLastMessage;

        [Space(5)]
        [Header("Instruction System References")]
        [SerializeField] private InstructionUISystem instructionSystem;
        [SerializeField] private bool isInstructionActive = false;


        [Space(5)]
        [Header("Toaster System References")]
        [SerializeField] private ToasterSystem toasterSystem;

        public bool InstructionActive() => isInstructionActive;

        public bool DialogActive() => isDialogActive;

        [Space(5)]
        [SerializeField] private NPC_Dialog currentNpc;

        private GamePlayerManager playerManager;

        #region Unity Methods

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            playerManager = GamePlayerManager.instance;
           

            if (playerManager != null)
            {
                Debug.Log("Game player manager found");
            }
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void Update()
        {
            if (playerManager != null && playerManager.gameState == MetaConstants.GameState.PUZZLE)
            {
                ControllerPuzzleMovement();
            }
        }

        private void LateUpdate()
        {
            SetCrossHairWorldPosition();

            if (playerManager != null && playerManager.gameState == MetaConstants.GameState.PUZZLE)
            {
                PuzzleCrossHairMovement();
            }
        }

        #endregion


        #region Event Registrations

        private void RegisterEvents()
        {
            dialogSystem.OnDialogStateUpdate += (bool _value) => isDialogActive = _value;

            instructionSystem.OnInstructionStateUpdate += (bool _value) => isInstructionActive = _value;
        }

        private void UnregisterEvents()
        {
            dialogSystem.OnDialogStateUpdate -= (bool _value) => isDialogActive = _value;

            instructionSystem.OnInstructionStateUpdate -= (bool _value) => isInstructionActive = _value;
        }

        #endregion


        #region Dialog System Section

        public void ShowDialog(string _message,NPC_Dialog npc, DialogInteraction _dialogInteraction)
        {
            activeDialogInteraction = _dialogInteraction;
            dialogSystem.UpdateText(_message);
            dialogSystem.Show();

            showingLastMessage = activeDialogInteraction.LastMessageShown;

            currentNpc = npc;
        }

        public void ConvoShowDialog(string _message, NPC_Dialog npc, ConversationInteraction _dialogInteraction)
        {
            activeConvoInteraction = _dialogInteraction;
            activeConvoInteraction.isActive = true;
            dialogSystem.UpdateText(_message);
            dialogSystem.Show();

            showingLastMessage = activeConvoInteraction.LastMessageShown;

            currentNpc = npc;
        }

        public List<ConvoUI> ShowOptions(List<ConvoNodeSO> convoNodes)
        {
             return dialogSystem.CreateOptions(convoNodes);
        }

        public void PopNextMessage()
        {
            if (activeDialogInteraction == null)
            {
                activeConvoInteraction.PopNextMessage();
            }
            else
            {
                activeDialogInteraction.PopNextMessage();
            }
        }

        public void UpdateDialog(string _message)
        {
            dialogSystem.UpdateText(_message);
        }

        public void SetLastMessageStatus(bool _value)
        {
            showingLastMessage = _value;
        }

        public void HideDialog()
        {
            activeConvoInteraction.isActive = false;
            activeDialogInteraction = null;

            dialogSystem.Hide();

            currentNpc.EndConversation();
            currentNpc = null;

        }

        #endregion


        #region Instruction System Section

        public void ShowInstructionBox(string message)
        {
            instructionSystem.UpdateText(message);
            instructionSystem.Show();
        }

        public void HideInstructionBox()
        {
            instructionSystem.Hide();
        }

        #endregion


        #region Cross-Hair Update Handling Section 

        private void SetCrossHairWorldPosition()
        {
            Camera cam = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;
            crossHairWorldPos = cam.ScreenToWorldPoint(new Vector3(crossHair.position.x, crossHair.position.y, 2f));

        }

        private void ControllerPuzzleMovement()
        {
            if (playerManager._input.isConsoleNavigateActive)
            {
                Vector2 inputVector = playerManager._input.consoleNavigate.normalized;
                Vector2 moveVector = new Vector2(inputVector.x, inputVector.y) * InputDeviceConstants.GP_RightStickMoveSpeed * Time.deltaTime;
                crossHair.anchoredPosition += moveVector;
            }
        }

        private void PuzzleCrossHairMovement()
        {
            if (playerManager._input.navigate != Vector2.zero)
            {
                //Convert viewport position to screen position
                Vector3 screenPos = playerManager._input.navigate;

                // Set the position of the RectTransform
                crossHair.position = screenPos;
            }
        }

       
        #endregion


        #region Toaster System Section

        public void AddToasterMessage(string _message)
        {
            toasterSystem.AddToasterMessage(_message);
        }

        #endregion
    }
}
