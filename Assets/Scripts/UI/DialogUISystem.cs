using KrazyKrakenGames.DetectiveGame.Conversations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(2)]
    public class DialogUISystem : MonoBehaviour
    {
        [SerializeField] private GameObject dialogBox;
        [SerializeField] private TextMeshProUGUI speakerText;
        [SerializeField] private TextMeshProUGUI messageText;

        [SerializeField] private Transform optionsParent;
        [SerializeField] private ConvoUI convoOptionPrefab;

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

        public void UpdateText(ConvoMessageSO _convoMessage)
        {
            speakerText.text = _convoMessage.speakerName;
            messageText.text = _convoMessage.message;
        }

        public List<ConvoUI> CreateOptions(List<ConvoNodeSO> siblings)
        {
            List<ConvoUI> optionList = new List<ConvoUI>();
            foreach(var sibling in siblings)
            {
                var convoUI = Instantiate(convoOptionPrefab, optionsParent);
                convoUI.ShowMessage(sibling); 
                optionList.Add(convoUI);
            }

            return optionList;
        }
    }
}
