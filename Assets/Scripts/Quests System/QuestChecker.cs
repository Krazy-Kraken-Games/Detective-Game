using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.QuestSystem
{
    /// <summary>
    /// This script will be responsible for only checking if the subsequent quests are completed
    /// in game. 
    /// </summary>
    public class QuestChecker : MonoBehaviour
    {
        [SerializeField] private QuestTrigger questTrigger;

        [SerializeField] private bool Completed = false;

        public UnityEvent DefaultEvents;
        public UnityEvent CompletedEvents;

        private void Start()
        {
            questTrigger.CompletedQuestEventsFired.AddListener(OnQuestCompleted);
        }

        private void OnQuestCompleted()
        {
            Completed = true;

            OnQuestCheck();
        }

        public void OnQuestCheck()
        {
            if (!Completed)
            {
                DefaultEvents.Invoke();
            }
            else
            {
                CompletedEvents.Invoke();
            }

        }
    }
}
