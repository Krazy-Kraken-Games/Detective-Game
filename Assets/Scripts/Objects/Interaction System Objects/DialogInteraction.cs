using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class DialogInteraction : InteractableObject
    {
        [SerializeField] private Transform lookAt;
        private string message;

        [SerializeField] private NPC_Dialog npc;
        public override void Interact()
        {
            message = "Dialog interaction detected from player";
            Debug.Log(message);

            CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);

            Invoke("StartConversation", 0.8f);
        }

        private void StartConversation()
        {
            npc.StartConversation();
            UIManager.instance.ShowDialog(message,npc);
        }
    }
}
