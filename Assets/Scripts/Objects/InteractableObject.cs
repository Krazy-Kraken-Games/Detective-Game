using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class InteractableObject : MonoBehaviour
    {
        public InteractableType type;
        public TriggerBox triggerBox;

        public void PickUpObject()
        {
            //Before destroying, add logic to update inventory

            Destroy(triggerBox.gameObject);
            Destroy(gameObject);
        }
    }
}
