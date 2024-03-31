using KrazyKrakenGames.Managers;
using UnityEngine;
using static KrazyKrakenGames.Global.MetaConstants;

namespace KrazyKrakenGames
{
    [DefaultExecutionOrder(12)]
    public class KrakenDebugger : MonoBehaviour
    {
        public static KrakenDebugger Instance = null;

        public bool allowDebugging = true;

        [SerializeField] private GamePlayerManager playerManager;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
           playerManager = GamePlayerManager.instance;
        }

        #region Update Game Mode Debug Buttons Handling

        public void SetToGameMode()
        {
            if (playerManager != null)
            {
                playerManager.UpdateMode(GameState.NORMAL);
                playerManager.UpdateInputMode(PlayerInputMode.UNLOCKED);
            }
        }

        public void SetToModelViewerMode()
        {
            if (playerManager != null)
            {
                playerManager.UpdateMode(GameState.VIEWER);
                playerManager.UpdateInputMode(PlayerInputMode.OBJECTVIEWER);
            }
        }

        public void SetToPuzzleMode()
        {
            if (playerManager != null)
            {
                playerManager.UpdateMode(GameState.PUZZLE);
                playerManager.UpdateInputMode(PlayerInputMode.PUZZLE);
            }
        }

        #endregion
    }
}
