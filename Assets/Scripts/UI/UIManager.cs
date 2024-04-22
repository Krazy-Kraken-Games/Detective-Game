using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Managers;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(-1)]
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        [Header("Cross-Hair Reference")]
        [SerializeField] private RectTransform crossHair;

        [Space(5)]
        [Header("Dialog System References")]
        [SerializeField] private DialogUISystem dialogSystem;
        [SerializeField] private bool isDialogActive = false;

        public bool DialogActive() => isDialogActive;

        [SerializeField] private NPC_Dialog currentNpc;

        private GamePlayerManager playerManager;

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
            if(playerManager != null && playerManager._input.raycaster != Vector2.zero)
            {
                // Convert viewport position to screen position
                Vector3 screenPos = playerManager._input.raycaster;

                // Set the position of the RectTransform
                crossHair.position = screenPos;
            }
        }


        private void RegisterEvents()
        {
            dialogSystem.OnDialogStateUpdate += (bool _value) => isDialogActive = _value;
        }

        private void UnregisterEvents()
        {
            dialogSystem.OnDialogStateUpdate -= (bool _value) => isDialogActive = _value;
        }


        #region Dialog System Section

        public void ShowDialog(string _message,NPC_Dialog npc)
        {
            dialogSystem.UpdateText(_message);
            dialogSystem.Show();

            currentNpc = npc;
        }

        public void HideDialog()
        {
            dialogSystem.Hide();

            currentNpc.EndConversation();
            currentNpc = null;

        }

        #endregion
    }
}
