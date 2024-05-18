using KrazyKrakenGames.DetectiveGame.QuestSystem;
using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    public class QuestInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI information;

        public void PopulateDisplay(Quest _quest)
        {
            information.text = _quest.Title;
        }
    }
}
