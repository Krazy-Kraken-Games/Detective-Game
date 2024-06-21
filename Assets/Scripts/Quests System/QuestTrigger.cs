using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.QuestSystem
{
    /// <summary>
    /// The quest trigger will act as an intermiedate to determine
    /// when the quest associated will be trigger, what are the condition listeners for it
    /// and when the quest will be completed
    /// </summary>
    /// 

    [DefaultExecutionOrder(2)]
    public class QuestTrigger : MonoBehaviour
    {
        public QuestSO questData;

        public Quest activeQuest;

        public List<QuestSegment> questSegments = new List<QuestSegment>();

        private QuestManager questManager;

        public UnityEvent CompletedQuestEventsFired;

        private void Awake()
        {
            CreateQuest();
        }

        public void CreateQuest()
        {
            activeQuest =
               new Quest(questData.ID, questData.Title, questData.Status, this);

            activeQuest.Segments = questSegments;

            questManager = QuestManager.instance;

            activeQuest.OnQuestCompleteEvent?.AddListener(OnActiveQuestCompleted);
        }

        public void QuestSegmentComplete(int _segmentID)
        {
            foreach(QuestSegment segment in activeQuest.Segments)
            {
                if(segment.SegmentID == _segmentID)
                {
                    segment.SegmentComplete();
                }
            }
        }

        public void StartQuest()
        {
            if (questManager != null)
            {
                questManager.AddQuest(activeQuest);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var collidedWith = other.gameObject;

            if(activeQuest.Status == QuestStatus.INACTIVE 
                && collidedWith.tag == MetaConstants.PlayerTag)
            {
                //Activate the quest if and only if the quest is inactive
                Debug.Log($"Starting quest:{activeQuest.Title}");
                StartQuest();
            }
        }


        private void OnActiveQuestCompleted(Quest _quest)
        {
            activeQuest.OnQuestCompleteEvent?.RemoveAllListeners();

            CompletedQuestEventsFired?.Invoke();
        }

    }
}
