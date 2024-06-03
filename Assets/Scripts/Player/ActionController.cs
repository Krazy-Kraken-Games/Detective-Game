using Cinemachine;
using KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.UI;
using StarterAssets;
using System;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Player
{
    [DefaultExecutionOrder(2)]
    public class ActionController : MonoBehaviour
    {
        public static ActionController Instance = null;

        [Header("Game Player Manager reference")]
        [SerializeField] private GamePlayerManager playerManager;
        [SerializeField] private bool isInputAllowed;
        [SerializeField] private ThirdPersonPlayer gamePlayer;
        [SerializeField] private StarterAssetsInputs _input;

        private Camera mainCamera;
        [SerializeField] private GameState currentGameState;

        [SerializeField] private LayerMask pieceLayer = 1 << 15;
        [SerializeField] private LayerMask investigateClueLayer = 1 << 16;
        [SerializeField] private LayerMask investigateObjectLayer = 1 << 17;
        [SerializeField] private LayerMask combinedLayerMask;


        //TO BE MOVED INTO PUZZLE MANAGER
        public PuzzlePiece selectedPuzzlePiece;


        public Action<Vector2> OnMoveInputEvent;

        #region Unity Methods

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            gamePlayer = GetComponent<ThirdPersonPlayer>();
            _input = gamePlayer.Input;

            playerManager = GamePlayerManager.instance;

            if (playerManager != null)
            {
                playerManager.OnPlayerInputModeChangedEvent += OnPlayerInputModeChangedEventHandler;
                playerManager.OnGameStateChangedEvent += OnGameModeChangeEventHandler;

                OnPlayerInputModeChangedEventHandler(playerManager.playerInputMode);

                OnGameModeChangeEventHandler(playerManager.gameState);
            }

            
            mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            if (playerManager != null)
            {
                playerManager.OnPlayerInputModeChangedEvent -= OnPlayerInputModeChangedEventHandler;
                playerManager.OnGameStateChangedEvent -= OnGameModeChangeEventHandler;
            }
        }


        private void Update()
        {
            //Entry point to handle kraken debugger input

            if(KrakenDebugger.Instance != null && KrakenDebugger.Instance.allowDebugging)
            {
                HandleKrakenDebuggerEntry();
            }

            if (!isInputAllowed) return;

            CancelInputHandling();

            HandleInventoryInputButton();

            InteractionInputHandling();

            if(playerManager.gameState == GameState.INVENTORY)
            {
                InvestigationRaycastHandling();
            }

            MovementInputHandling();
        }

        #endregion

        #region Events and Variable Registrations

        private void OnPlayerInputModeChangedEventHandler(PlayerInputMode playerInputMode)
        {
            if (playerInputMode == PlayerInputMode.SECONDARY)
            {
                isInputAllowed = true;
            }
            else
            {
                isInputAllowed = false;
            }
        }

        private void OnGameModeChangeEventHandler(GameState gameState)
        {
            currentGameState = gameState;
        }

        #endregion

        private void HandleInventoryInputButton()
        {
            if (_input.inventory)
            {
                UIManager.instance.ToggleInventory();

                _input.inventory = false;
            }
        }
        public void CancelInputHandling()
        {
            if (_input.cancel)
            {
                _input.cancel = false;

                //Check if dialog box is currently open
                if (UIManager.instance.DialogActive())
                {
                    //Converting this to handle if its last message of conversation

                    if (UIManager.instance.LastMessageShown)
                    {
                        UIManager.instance.HideDialog();
                        SwitchControlBackToPrimaryController();
                    }
                    else
                    {
                        //Pop next message from queue
                        UIManager.instance.PopNextMessage();
                    }

                    return;
                }

                //Check if instruction box is currently open
                if (UIManager.instance.InstructionActive())
                {
                    UIManager.instance.HideInstructionBox();

                    return;
                }

                if (selectedPuzzlePiece != null)
                {
                    //Drop functionality
                    selectedPuzzlePiece.UnSelect(this);
                    selectedPuzzlePiece = null;
                }
                else
                {
                    SwitchControlBackToPrimaryController();
                }
            }
        }

        private void InteractionInputHandling()
        {
            if (_input.interact)
            {
                _input.interact = false;

                //If game state is currently puzzle, then we take raycasting input

                if (currentGameState == GameState.PUZZLE)
                {
                    //Allow raycasting
                    LookInputHandling();
                }
            }
        }


        private void SwitchControlBackToPrimaryController()
        {
            CameraManager.instance.SetState(GameCameraState.PRIMARY);
            playerManager.UpdateInputMode(PlayerInputMode.PRIMARY);
            playerManager.UpdateMode(GameState.NORMAL);
        }

        #region Raycast Input Handling

        private void InvestigationRaycastHandling()
        {
            if(_input != null)
            {
                Vector3 crossHairWorldPosition = UIManager.instance.CrossHairWorldPosition;

                Camera cam = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;

                Vector3 direction = crossHairWorldPosition - cam.gameObject.transform.position;
                Ray ray = new Ray(cam.gameObject.transform.position, direction);

                RaycastHit hit;

                combinedLayerMask = investigateClueLayer | investigateObjectLayer;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayerMask))
                {
                    int layerIndex = hit.collider.gameObject.layer;
                    if ((investigateClueLayer.value & (1 << layerIndex)) != 0)
                    {
                        Debug.Log("We nailed a clue!");
                        UIManager.instance.OnInvestigationClueHit();
                    }
                    else if ((investigateObjectLayer.value & (1 << layerIndex)) != 0)
                    {
                        Debug.Log("We hit on an object");
                        UIManager.instance.ResetInvestigationSliderValue();
                    }
                }
                else
                {
                    UIManager.instance.ResetInvestigationSliderValue();
                }
            }
        }

        private void LookInputHandling()
        {
            if (_input != null)
            {
                Vector3 crossHairWorldPosition = UIManager.instance.CrossHairWorldPosition;

                Camera cam = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;
               
                Vector3 direction = crossHairWorldPosition - cam.gameObject.transform.position;
                Ray ray = new Ray(cam.gameObject.transform.position, direction);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, pieceLayer))
                {
                    Debug.Log($"Object interacted with:{hit.collider.gameObject.name}");

                    if (selectedPuzzlePiece != null)
                    {
                       selectedPuzzlePiece.UnSelect(this);
                       selectedPuzzlePiece = null;
                    }
                    selectedPuzzlePiece = hit.collider.gameObject.GetComponent<PuzzlePiece>();
                    selectedPuzzlePiece.Select(this);
                }
            }
        }

        #endregion

        #region Movement Input Handling

        private void MovementInputHandling()
        {
            if(_input.move != Vector2.zero)
            {
                Vector3 forward = mainCamera.transform.forward;
                Vector3 right = mainCamera.transform.right;
                forward.y = 0;
                right.y = 0;
                forward = forward.normalized;
                right = right.normalized;

                Vector3 forwardRelativeVerticalMovement = _input.move.y * forward;
                Vector3 rightRelativeHorizontalMovement = _input.move.x * right;

                Vector3 cameraRelativeMovement =
                            forwardRelativeVerticalMovement + rightRelativeHorizontalMovement;

                Vector2 movementVector = new Vector2(cameraRelativeMovement.x, cameraRelativeMovement.z);

                OnMoveInputEvent?.Invoke(movementVector);
            }
        }

        #endregion

        #region Debugging
        private void HandleKrakenDebuggerEntry()
        {
            if (_input.kraken)
            {
                _input.kraken = false;

                KrakenDebugger.Instance.ShowMenu();
                _input.cursorLocked = false;
                _input.cursorInputForLook = false;
            }
        }


        private void OnDrawGizmos()
        {
            if (!isInputAllowed) return;

            if(currentGameState == GameState.PUZZLE)
            {

                Vector3 crossHairWorldPosition = UIManager.instance.CrossHairWorldPosition;

                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(crossHairWorldPosition, 0.1f);

                Camera cam = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(cam.gameObject.transform.position, 0.1f);

                Vector3 direction = crossHairWorldPosition - cam.gameObject.transform.position;
                Ray newRay = new Ray(cam.gameObject.transform.position, direction);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(newRay);
            }
        }

        #endregion
    }
}
