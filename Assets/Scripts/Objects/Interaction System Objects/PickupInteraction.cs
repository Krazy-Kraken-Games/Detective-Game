using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class PickupInteraction : InteractableObject
    {
        [SerializeField] private StoryObjectSO objectData;
        [SerializeField] private InvestigationModelSO investigationData;
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

            if (objectData != null)
            {
                var message = $"{objectData.ObjectName} has been picked up";
                UIManager.instance.AddToasterMessage(message);

                if (investigationData != null)
                {
                    InventoryManager.Instance.AddModelData(investigationData);
                }
            }
            else
            {
                Debug.LogWarning("Pick up object missing data");
            }

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
