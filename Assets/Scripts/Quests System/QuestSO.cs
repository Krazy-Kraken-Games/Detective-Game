using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.QuestSystem
{
    public enum QuestStatus
    {
        INACTIVE = 0,
        INPROGRESS = 1, //For now, will also serve as status ACTIVE
        COMPLETED = 2
    }

    [CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Create Quest")]
    public class QuestSO: ScriptableObject
    {
        //Ensure ID is not 0
        public int ID;
        public string Title;

        public QuestStatus Status;
    }

    [System.Serializable]
    public class Quest
    {
        public int ID;
        public string Title;
        public QuestStatus Status;
        public QuestTrigger QuestTrigger;

        public List<QuestSegment> Segments;

        [SerializeField] private int SegmentsCompleted;

        public Action<Quest,QuestStatus> OnQuestStatusChange;
        public UnityEvent<Quest> OnQuestCompleteEvent = new UnityEvent<Quest>();

        public Quest(int _ID, string _title, QuestStatus _status, QuestTrigger _questTrigger)
        {
            ID = _ID;
            Title = _title;
            Status = _status;
            QuestTrigger = _questTrigger;
        }

        private void SetQuestState(QuestStatus _newStatus)
        {
            Status = _newStatus;
            OnQuestStatusChange?.Invoke(this,Status);
        }

        public void StartingQuest()
        {
            SetQuestState(QuestStatus.INPROGRESS);

            foreach (QuestSegment segment in Segments)
            {
                segment.OnSegmentComplete.AddListener(SegmentComplete);
            }
        }

        public void UnregisterListeners()
        {
            foreach (QuestSegment segment in Segments)
            {
                segment.OnSegmentComplete.RemoveAllListeners();
            }
        }

        private void SegmentComplete(QuestSegment segment)
        {
            SegmentsCompleted++;

            segment.OnSegmentComplete.RemoveAllListeners();

            if(SegmentsCompleted == Segments.Count)
            {
                SetQuestState(QuestStatus.COMPLETED);
                OnQuestCompleteEvent?.Invoke(this);
            }
        }
        
    }
}
