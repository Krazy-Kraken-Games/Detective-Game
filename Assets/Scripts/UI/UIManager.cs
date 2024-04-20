using KrazyKrakenGames.DetectiveGame.AI;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(-1)]
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance = null;

        [Header("Dialog System References")]
        [SerializeField] private DialogUISystem dialogSystem;
        [SerializeField] private bool isDialogActive = false;

        public bool DialogActive() => isDialogActive;

        [SerializeField] private NPC_Dialog currentNpc;

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
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
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
