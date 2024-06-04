using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Managers
{
    /// <summary>
    /// This singleton script will be responsible for handling player mode and 
    /// firing listeners, updating player mode and firing other related events
    /// </summary>

    [DefaultExecutionOrder(-2)]
    public class GamePlayerManager : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Singleton for GamePlayerMangaer
        /// </summary>
        public static GamePlayerManager instance = null;

        //Current Game Mode and Event to fire on mode change
        public GameState gameState;
        public Action<GameState> OnGameStateChangedEvent;

        //Player Input lock/unlock
        public PlayerInputMode playerInputMode;
        public Action<PlayerInputMode> OnPlayerInputModeChangedEvent;

        //[Header("Game Systems References")]
        //[SerializeField] private PPE_DetectiveMode detectiveModeObject;


        [Space(5)]
        [Header("Input References")]
        public StarterAssetsInputs _input;

#if ENABLE_INPUT_SYSTEM
        public PlayerInput _playerInput;
#endif

        #endregion


        #region UNITY_FUNCTIONS
        private void Awake()
        {
            if (instance == null)
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
            UpdateMode(GameState.NORMAL);
            //detectiveModeObject.OnDetectiveModeActivatedEvent += OnDetectiveModeActivatedEventHandler;
        }

        private void OnDestroy()
        {
            //detectiveModeObject.OnDetectiveModeActivatedEvent -= OnDetectiveModeActivatedEventHandler;
        }

        #endregion


        #region Player Mode Change Handling
        /// <summary>
        /// Updates current mode based on parameter and fires event
        /// </summary>
        /// <param name="mode">Mode to change to</param>
        public void UpdateMode(GameState mode)
        {
            gameState = mode;

            if(mode == GameState.PUZZLE || mode == GameState.INVENTORY)
            {
                _input.cursorLocked = false;
                _input.SetCursorInGame(_input.cursorLocked);
            }
            else if(mode == GameState.SHOOT)
            {
                _input.cursorLocked = false;
                _input.SetCursorInGame(_input.cursorLocked);


            }
            else
            {
                _input.cursorLocked = true;
                _input.SetCursorInGame(_input.cursorLocked);
            }
            OnGameStateChangedEvent?.Invoke(gameState);
        }
        #endregion


        #region Lock/Unlock Player Input

        /// <summary>
        /// Updates the current input mode and fires related event
        /// </summary>
        /// <param name="mode">Mode to set input to</param>
        public void UpdateInputMode(PlayerInputMode mode)
        {
            playerInputMode = mode;

            OnPlayerInputModeChangedEvent?.Invoke(playerInputMode);
        }
        #endregion

        #region Listeners to other gameplay elements
        /// <summary>
        /// Event listener that fires when detective mode is activated from the gameplay script
        /// </summary>
        /// <param name="isActive">Denotes is detective mode active</param>
        private void OnDetectiveModeActivatedEventHandler(bool isActive)
        {
            if (isActive)
            {
                UpdateMode(GameState.DETECTIVE);
            }
            else
            {
                UpdateMode(GameState.NORMAL);
            }
        }

        #endregion
    }
}
