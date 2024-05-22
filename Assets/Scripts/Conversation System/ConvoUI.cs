using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KrazyKrakenGames.DetectiveGame.Conversations
{
    public enum UI_TYPE
    {
        MESSAGE = 0,
        BACK = 1   ///Refers to back button
    }

    public class ConvoUI : MonoBehaviour
    {
        public UI_TYPE type;
        public Conversation conversation;
        public ConvoNodeSO data;
        public TextMeshProUGUI message;
        public Button button;

        public void Populate(ConvoNodeSO _nodeData, Conversation _conversation)
        {
            conversation = _conversation;
            data = _nodeData;
            message.text = $"{_nodeData.messageData.speakerName}:{_nodeData.messageData.message}";
        }

        public void ShowMessage(ConvoNodeSO _nodeData)
        {
            data = _nodeData;
            message.text = $"{_nodeData.messageData.speakerName}:{_nodeData.messageData.message}";
        }

        public void Highlighted()
        {
            button.Select();

            button.OnSelect(null);
        }

        public void UnHighlight()
        {
            EventSystem.current.SetSelectedGameObject(null);
            button.OnDeselect(null);
        }

        public void OnButtonClicked()
        {
            if (type == UI_TYPE.MESSAGE)
            {

                if (data.Children.Count > 0)
                {
                    Debug.Log($"{data.name} Has children to show");
                    conversation.ShowChildren(data);
                }
                else
                {
                    Debug.Log($"{data.name} Has no children to show");
                }
            }
            else if( type == UI_TYPE.BACK)
            {
                Debug.Log("Going back to previous tree node");
                conversation.BackToActiveTree();
            }
        }
    }
}
