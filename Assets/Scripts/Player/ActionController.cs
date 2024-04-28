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
        [Header("Game Player Manager reference")]
        [SerializeField] private GamePlayerManager playerManager;
        [SerializeField] private bool isInputAllowed;
        [SerializeField] private ThirdPersonPlayer gamePlayer;
        [SerializeField] private StarterAssetsInputs _input;

        [SerializeField] private GameState currentGameState;

        [SerializeField] private LayerMask pieceLayer = 1 << 15;


        //TO BE MOVED INTO PUZZLE MANAGER
        public PuzzlePiece selectedPuzzlePiece;


        public Action<Vector2> OnMoveInputEvent;

        #region Unity Methods

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

            InteractionInputHandling();

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


        private void CancelInputHandling()
        {
            if (_input.cancel)
            {
                _input.cancel = false;

                //Check if dialog box is currently open
                if (UIManager.instance.DialogActive())
                {
                    UIManager.instance.HideDialog();
                }

                //Check if instruction box is currently open
                if (UIManager.instance.InstructionActive())
                {
                    UIManager.instance.HideInstructionBox();
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

        private void LookInputHandling()
        {
            if (_input != null)
            {
                Vector2 startPosition = new Vector2(_input.raycaster.x, _input.raycaster.y);
                Ray ray = Camera.main.ScreenPointToRay(startPosition);

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
                OnMoveInputEvent?.Invoke( _input.move);
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
                Vector2 startPosition = new Vector2(_input.raycaster.x, _input.raycaster.y);
                Ray ray = Camera.main.ScreenPointToRay(startPosition);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(ray);
            }
        }

        #endregion
    }
}
