using System;
using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(2)]
    public class InstructionUISystem : MonoBehaviour
    {
        [SerializeField] private GameObject instructionBox;
        [SerializeField] private TextMeshProUGUI messageText;

        public Action<bool> OnInstructionStateUpdate;

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            instructionBox.SetActive(true);

            OnInstructionStateUpdate?.Invoke(true);
        }

        public void Hide()
        {
            instructionBox.SetActive(false);

            OnInstructionStateUpdate?.Invoke(false);
        }

        public void UpdateText(string _message)
        {
            messageText.text = _message;
        }
    }
}
