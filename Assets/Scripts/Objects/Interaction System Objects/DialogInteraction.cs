using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Managers;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class DialogInteraction : InteractableObject
    {
        public override void Interact()
        {
            Debug.Log("Dialog interaction through inheritance");

            GamePlayerManager.instance.UpdateInputMode(MetaConstants.PlayerInputMode.PRIMARY);
        }
    }
}
