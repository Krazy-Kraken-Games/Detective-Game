using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    public class UIMessageScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI MessageText;

        public void SetMessage(string _message)
        {
            MessageText.text = _message;
        }
    }
}
