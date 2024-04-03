using KrazyKrakenGames.DetectiveGame.Managers;
using StarterAssets;
using System.Linq;
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

        #region Unity Methods

        private void Start()
        {
            gamePlayer = GetComponent<ThirdPersonPlayer>();
            _input = gamePlayer.Input;

            playerManager = GamePlayerManager.instance;

            if (playerManager != null)
            {
                playerManager.OnPlayerInputModeChangedEvent += OnPlayerInputModeChangedEventHandler;

                OnPlayerInputModeChangedEventHandler(playerManager.playerInputMode);
            }
        }

        private void OnDestroy()
        {
            if (playerManager != null)
            {
                playerManager.OnPlayerInputModeChangedEvent -= OnPlayerInputModeChangedEventHandler;
            }
        }


        private void Update()
        {
            if (!isInputAllowed) return;

            CancelInputHandling();

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

        #endregion


        private void CancelInputHandling()
        {
            if (_input.cancel)
            {
                _input.cancel = false;
                SwitchControlBackToPrimaryController();
            }
        }


        private void SwitchControlBackToPrimaryController()
        {
            CameraManager.instance.SetState(GameCameraState.PRIMARY);
            playerManager.UpdateInputMode(PlayerInputMode.PRIMARY);
        }
    }
}
