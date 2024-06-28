using Cinemachine;
using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Conversations;
using KrazyKrakenGames.DetectiveGame.Gameplay;
using KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles;
using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.ThesisProject.GameModel;
using System.Collections.Generic;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(-1)]
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        [Header("Manager References")]
        [SerializeField] private CameraManager cameraManager;

        [Header("UI Screen References")]
        [SerializeField] private UIScreenSwitcher mainScreens;

        [Header("Cross-Hair Reference")]
        public RectTransform crossHair;
        private Vector3 crossHairWorldPos;
        private Vector2 crossHairDefaultPosition;

        [Header("Investigation Slider Reference")]
        [SerializeField] private UnityEngine.UI.Slider investigationSlider;
        [SerializeField] private float investigationSliderMaxValue;
        private float investigationSliderValue;
        [SerializeField] private InvestigationClue currentClue;

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

        [Space(5)]
        [Header("Inventory UI")]
        [SerializeField] private Model modelParent;
        [SerializeField] private GameObject inventoryUI;
        private bool isInventoryActive;

        [Space(5)]
        [Header("Message Screen")]
        [SerializeField] private UIMessageScreen messageScreen;
        [SerializeField] private bool isMessageActive;

        public bool MessageScreenActive => isMessageActive;

        public bool InventoryActive => isInventoryActive;

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
            cameraManager = CameraManager.instance;

            crossHairDefaultPosition = crossHair.anchoredPosition;

            if (investigationSlider != null)
            {
                investigationSliderMaxValue = MetaConstants.InvestigationSliderMaxValue;
                investigationSlider.maxValue = investigationSliderMaxValue;
            }
            else
            {
                Debug.LogWarning("Missing Investigation Slider", gameObject);
            }

            RegisterEvents();

            if(playerManager != null)
            {
                OnGameStateChangedEventHandler(playerManager.gameState);
            }

            HideInventory();

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

            if (playerManager != null && 
                (playerManager.gameState == GameState.PUZZLE) || (playerManager.gameState == GameState.INVENTORY))
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

            if (playerManager != null)
            {
                playerManager.OnGameStateChangedEvent += OnGameStateChangedEventHandler;
            }

            if(cameraManager != null)
            {
                cameraManager.OnStateChangeEvent += OnCameraStateUpdatedHandler;
            }
        }

        private void UnregisterEvents()
        {
            dialogSystem.OnDialogStateUpdate -= (bool _value) => isDialogActive = _value;

            instructionSystem.OnInstructionStateUpdate -= (bool _value) => isInstructionActive = _value;

            if (playerManager != null)
            {
                playerManager.OnGameStateChangedEvent -= OnGameStateChangedEventHandler;
            }

            if (cameraManager != null)
            {
                cameraManager.OnStateChangeEvent += OnCameraStateUpdatedHandler;
            }

        }

        private void OnGameStateChangedEventHandler(MetaConstants.GameState _newState)
        {
            if(_newState == GameState.NORMAL)
            {
                //Hide crosshair
                HideCrossHair();
            }
            else if(_newState == GameState.SHOOT)
            {
                //Show crosshair for shoot
                ShowCrossHair();
            }
            else if(_newState == GameState.PUZZLE)
            {
                //Show crosshair for puzzle
                ShowCrossHair();
            }
            else if(_newState == GameState.INVENTORY)
            {
                ShowCrossHair();
            }
        }

        private void OnCameraStateUpdatedHandler(GameCameraState _newCameraState)
        {
            
        }

        #endregion


        #region Dialog System Section

        public void ShowDialog(ConvoMessageSO _message,NPC_Dialog npc, DialogInteraction _dialogInteraction)
        {
            activeDialogInteraction = _dialogInteraction;
            dialogSystem.UpdateText(_message);
            dialogSystem.Show();

            showingLastMessage = activeDialogInteraction.LastMessageShown;

            currentNpc = npc;
        }

        public void ConvoShowDialog(ConvoMessageSO _message, NPC_Dialog npc, ConversationInteraction _dialogInteraction)
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

        public void UpdateDialog(ConvoMessageSO _message)
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
            activeConvoInteraction.Reset();

            dialogSystem.Hide();

            if (currentNpc != null)
            {
                currentNpc.EndConversation();
                currentNpc = null;
            }
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

        private void HideCrossHair()
        {
            crossHair.gameObject.SetActive(false);
            crossHair.anchoredPosition = crossHairDefaultPosition;
        }

        private void ShowCrossHair()
        {
            crossHair.gameObject.SetActive(true);
            crossHair.anchoredPosition = crossHairDefaultPosition;
        }
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

        #region Message Screen Section

        public void ToggleMessageScreen(string message = "")
        {
            if (!isMessageActive)
            {
                GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);
                messageScreen.SetMessage(message);
                mainScreens.SetState(ScreenState.Screen3);
                isMessageActive = true;
            }
            else
            {
                //Hide Message Screen
                mainScreens.SetState(ScreenState.Screen1);
                isMessageActive = false;
                GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.PRIMARY);
            }
        }

        #endregion

        #region Inventory Section

        public void ToggleInventory()
        {
            if (!isInventoryActive)
            {
                ShowInventory();
            }
            else
            {
                HideInventory();
            }
        }

        private void ShowInventory()
        {
            inventoryUI.SetActive(true);
            CameraManager.instance.SetState(GameCameraState.INVENTORY);
            playerManager.UpdateInputMode(PlayerInputMode.SECONDARY);
            playerManager.UpdateMode(GameState.INVENTORY);

            mainScreens.SetState(ScreenState.Screen2);

            isInventoryActive = true;
        }

        private void HideInventory() 
        {
            CameraManager.instance.SetState(GameCameraState.PRIMARY);
            playerManager.UpdateInputMode(PlayerInputMode.PRIMARY);
            playerManager.UpdateMode(GameState.NORMAL);

            mainScreens.SetState(ScreenState.Screen1);
            inventoryUI.SetActive(false);
            isInventoryActive = false;
        }


        #endregion

        #region Investigation UI Section

        public void OnInvestigationClueHit(InvestigationClue _clue)
        {
            investigationSlider.gameObject.SetActive(true);
            investigationSliderValue += Time.deltaTime;
            UpdateInvestigationSliderValue(investigationSliderValue);

            currentClue = _clue;
        }

        private void UpdateInvestigationSliderValue(float value)
        {
            if(value >= investigationSliderMaxValue)
            {
                if(currentClue != null)
                {
                    currentClue.ClueFound();
                    investigationSlider.gameObject.SetActive(false);
                }
            }
            else
            {
                investigationSlider.value = value;
            }
            
        }

        public void ResetInvestigationSliderValue()
        {
            investigationSliderValue = 0;
            investigationSlider.value = investigationSliderValue;
            investigationSlider.gameObject.SetActive(false);
            currentClue = null;
        }

        #endregion

        #region Handle Inventory Section

        public void SetInvestigationModel(InvestigationObject _object)
        {
            modelParent.SetNewActiveModel(_object);
        }

        private void ActivateInventory()
        {
            Debug.Log("Activate inventory");

            mainScreens.SetState(ScreenState.Screen2);
        }

        private void DeactivateInventory()
        {
            Debug.Log("Deactivate inventory");

            mainScreens.SetState(ScreenState.Screen1);
        }

        #endregion
    }
}
