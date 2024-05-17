using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class PickupInteraction : InteractableObject
    {
        public UnityEvent OnPickupComplete;
        
        public override void Interact()
        {
            base.Interact();
        }

        public void PickUpObject()
        {
            //Before destroying, add logic to update inventory
            triggerBox.gameObject.SetActive(false);
            gameObject.SetActive(false);

            OnPickupComplete?.Invoke();

            Invoke("DestroyObjects", 1f);
        }

        private void DestroyObjects()
        {
            Destroy(triggerBox.gameObject);
            Destroy(gameObject);
        }
    }
}
