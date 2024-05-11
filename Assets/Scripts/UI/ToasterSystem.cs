using KrazyKrakenGames.DetectiveGame.Utility;
using System;
using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(0)]
    public class ToasterSystem : MonoBehaviour
    {
        [SerializeField] private GameObject toasterParent;
        [SerializeField] private TextMeshProUGUI messageText;

        [SerializeField] private bool showToasterMessage = false;
        [SerializeField] private float showMessageForTime = 1.5f;


        [SerializeField] private MessageQueue<string> toasterMessages = new MessageQueue<string>();

        public Action<string> OnNewToasterMessageReceived;
       private void Start()
        {
            Hide();

            OnNewToasterMessageReceived += OnNewToasterMessageHandler;
        }

        private void OnDestroy()
        {
            OnNewToasterMessageReceived -= OnNewToasterMessageHandler;
        }

        public void Show()
        {
            showToasterMessage = false;
            toasterParent.SetActive(true);

            string message = toasterMessages.Pop();
            UpdateText(message);

            Invoke("Hide", showMessageForTime);
        }

        public void Hide()
        {
            toasterParent.SetActive(false);
            showToasterMessage = true;

            if (toasterMessages.Count > 0)
            {
                Show();
            }
        }

        public void UpdateText(string _message)
        {
            messageText.text = _message;
        }


        public void AddToasterMessage(string _message)
        {
            toasterMessages.Add(_message);
            OnNewToasterMessageReceived?.Invoke(_message);
        }

        private void OnNewToasterMessageHandler(string _message)
        {
            if (showToasterMessage)
            {
                Show();
            }
        }
        
    }
}
