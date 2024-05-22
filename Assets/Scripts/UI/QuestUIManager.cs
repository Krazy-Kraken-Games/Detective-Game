using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.QuestSystem;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    public class QuestUIManager : MonoBehaviour
    {
        public static QuestUIManager instance = null;

        [SerializeField] private Transform content;

        [SerializeField] private QuestManager questManager;

        [SerializeField] private QuestInfo questInfoPrefab;

        private Dictionary<int, QuestInfo> questInfos = new Dictionary<int, QuestInfo>();

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

        private void Start()
        {
            questManager = QuestManager.instance;

            if(questManager != null)
            {
                questManager.OnNewQuestStarted += OnNewQuestStartedHandler;
            }
        }

        private void OnDestroy()
        {
            if (questManager != null)
            {
                questManager.OnNewQuestStarted -= OnNewQuestStartedHandler;
            }
        }

        private void OnNewQuestStartedHandler(Quest _quest)
        {
            if (questInfos.ContainsKey(_quest.ID)) return;

            CreateNewQuestUI(_quest);
        }

        private void CreateNewQuestUI(Quest _quest)
        {
            var questInfo = Instantiate(questInfoPrefab, content);

            questInfo.PopulateDisplay(_quest);

            questInfos.Add(_quest.ID, questInfo);

            _quest.OnQuestCompleteEvent.AddListener(OnQuestComplete);
        }

        private void OnQuestComplete(Quest completedQuest)
        {
            Debug.Log("From UI, quest completed, delete prefab");

            if (questInfos.ContainsKey(completedQuest.ID))
            {
                Debug.Log("REmoved from UI too");
                var uiElement = questInfos[completedQuest.ID];
                questInfos.Remove(completedQuest.ID);

                UIManager.instance.AddToasterMessage($"{completedQuest.Title} Quest Completed!");

                Destroy(uiElement.gameObject);
            }

            completedQuest.OnQuestCompleteEvent.RemoveListener(OnQuestComplete);
        }
    }
}
