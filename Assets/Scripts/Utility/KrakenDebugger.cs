using KrazyKrakenGames.DetectiveGame.Managers;
using TMPro;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames
{
    [DefaultExecutionOrder(12)]
    public class KrakenDebugger : MonoBehaviour
    {
        public static KrakenDebugger Instance = null;

        public bool allowDebugging = true;

        [Header("Panel References")]
        [SerializeField] private GameObject statusPanel;
        [SerializeField] private GameObject gameMenuPanel;

        [Space(5)]
        [Header("Status Texts")]
        [SerializeField] private TextMeshProUGUI status;
        [SerializeField] private TextMeshProUGUI fpsText;

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
            HideMenu();
        }


        private void Update()
        {
            if (allowDebugging)
            {
                status.text = "Kraken Debugger is active";
                statusPanel.SetActive(true);
                ShowFPS();
            }
            else
            {
                status.text = "Kraken Debugger is inactive";
                statusPanel.SetActive(false);
            }
        }


        #region FPS Counter 

        /// <summary>
        /// Helper function to display FPS counter on screen
        /// </summary>
        private void ShowFPS()
        {
            if (fpsText != null)
            {
                float fps = (int)(1f / Time.deltaTime);

                fpsText.text = $"FPS: {fps}";
            }
            else
            {
                Debug.LogWarning("FPS counter text has not been added");
            }
        }

        #endregion

        #region Game Control Region


        public void ShowMenu()
        {
            gameMenuPanel.SetActive(true);
        }

        public void HideMenu()
        {
            gameMenuPanel.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        #endregion
    }
}
