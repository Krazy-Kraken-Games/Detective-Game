
using System;
using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.QuestSystem
{
    public enum QuestSegmentType
    {
        DEFAULT = 0,
        COLLECT = 1,
        DIALOG = 2,
        PUZZLE = 3,
        KILL = 4        //This could refer to killing AI beings, destroying objects in game
    }


    /// <summary>
    /// Class refers to each sub-section of a quest. A quest will have
    /// multiple quest segments to be completed by the player to complete
    /// the entire quest.
    /// </summary>
    /// 

    //TODO: We will get rid of this if its not needed
    [CreateAssetMenu(fileName = "Segment", menuName = "Quest System/Create Segment")]
    public class QuestSegmentSO: ScriptableObject
    {
        public QuestSegmentType Type;
        public bool isCompleted;
    }

    [System.Serializable]
    public class QuestSegment
    {
        public int SegmentID;
        public string Title;
        public QuestSegmentType Type;
        public bool isCompleted;

        //Reactions when the segment is completed
        public UnityEvent<QuestSegment> OnSegmentComplete;

        public void SegmentComplete()
        {
            isCompleted = true;
            OnSegmentComplete.Invoke(this);

            Debug.Log($"Segment complete: {Title}");

        }
    }
}
