using KrazyKrakenGames.DetectiveGame.QuestSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Managers
{
    /// <summary>
    /// A singleton manager serving to hold information about all the active
    /// quests.
    /// </summary>
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance = null;

        public Dictionary<int, Quest> activeQuests = new Dictionary<int, Quest>();

        public Action<Quest> OnNewQuestStarted;
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddQuest(Quest _quest)
        {
            if (activeQuests.ContainsKey(_quest.ID)) return;

            activeQuests.Add(_quest.ID, _quest);

            Debug.Log($"Added quest: {_quest.Title}");

            _quest.StartingQuest();

            OnNewQuestStarted?.Invoke(_quest);
        }

        public Quest GetQuest(int _Id)
        {
            return activeQuests[_Id];
        }
    }
}
