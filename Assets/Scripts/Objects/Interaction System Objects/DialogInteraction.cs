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
        public override void Interact()
        {
            message = "Dialog interaction through inheritance";
            Debug.Log(message);

            CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);

            Invoke("StartConversation", 0.8f);
        }

        private void StartConversation()
        {
            UIManager.instance.ShowDialog(message);
        }
    }
}
