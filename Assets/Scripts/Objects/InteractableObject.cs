using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// This will serve as a base interactable object class implementing interface IInteraction
    /// </summary>
    public class InteractableObject : MonoBehaviour, IInteraction
    {
        public InteractableType Type;
        public TriggerBox triggerBox;

        public InteractableType type
        {
            get
            {
                return Type;
            }
        }

       //TODO: This method will be moved to segregated and specialized pick up interaction obj script
        public void PickUpObject()
        {
            //Before destroying, add logic to update inventory

            Destroy(triggerBox.gameObject);
            Destroy(gameObject);
        }

        public void Interact()
        {

        }
    }
}
