using System;
using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(2)]
    public class DialogUISystem : MonoBehaviour
    {
        [SerializeField] private GameObject dialogBox;
        [SerializeField] private TextMeshProUGUI messageText;

        public Action<bool> OnDialogStateUpdate;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            dialogBox.SetActive(true);

            OnDialogStateUpdate?.Invoke(true);
        }

        public void Hide()
        {
            dialogBox.SetActive(false);

            OnDialogStateUpdate?.Invoke(false);
        }

        public void UpdateText(string _message)
        {
            messageText.text = _message;
        }
    }
}
