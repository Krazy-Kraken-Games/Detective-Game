using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class PickupInteraction : InteractableObject
    {
        public override void Interact()
        {
            Debug.Log("Pick up interaction through inheritance");

            base.Interact();
        }

        public void PickUpObject()
        {
            //Before destroying, add logic to update inventory

            Destroy(triggerBox.gameObject);
            Destroy(gameObject);
        }
    }
}
