using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections.Generic;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class DialogInteraction : InteractableObject
    {
        [SerializeField] private Transform lookAt;

        [SerializeField] private List<string> allMessages = new List<string>();
        [SerializeField] private int currentMessageIndex;
        [SerializeField] private bool isLastMessage = false;
        private string message;

        [SerializeField] private NPC_Dialog npc;

        public bool LastMessageShown => isLastMessage;

        [Space(10)]
        [Header("All Quest Conditions")]
        [SerializeField] private List<QuestCondition> allQuestConditions = new List<QuestCondition>();

        public override void Interact()
        {
            currentMessageIndex = 0;

            message = allMessages[currentMessageIndex];

            if (currentMessageIndex == allMessages.Count - 1)
            {
                OnLastMessageShown();
            }

            CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);

            Invoke("StartConversation", 0.8f);
        }

        private void StartConversation()
        {
            npc.StartConversation();
            UIManager.instance.ShowDialog(message,npc,this);
        }

        public void PopNextMessage()
        {
            currentMessageIndex++;
            message = allMessages[currentMessageIndex];

            if(currentMessageIndex == allMessages.Count - 1)
            {
                OnLastMessageShown();
            }

            UIManager.instance.UpdateDialog(message);
        }

        private void OnLastMessageShown()
        {
            isLastMessage = true;

            allQuestConditions[0].UpdateCondition(isLastMessage);
         
        }
    }
}
