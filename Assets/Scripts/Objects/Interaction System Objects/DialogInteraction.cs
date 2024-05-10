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

        public override void Interact()
        {
            currentMessageIndex = 0;

            message = allMessages[currentMessageIndex];

            if (currentMessageIndex == allMessages.Count - 1)
            {
                isLastMessage = true;
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
                isLastMessage = true;
            }

            UIManager.instance.UpdateDialog(message);
        }
    }
}
