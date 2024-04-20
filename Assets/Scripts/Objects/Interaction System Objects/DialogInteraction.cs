using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class DialogInteraction : InteractableObject
    {
        public override void Interact()
        {
            string message = "Dialog interaction through inheritance";
            Debug.Log(message);
            UIManager.instance.ShowDialog(message);

            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);
        }
    }
}
