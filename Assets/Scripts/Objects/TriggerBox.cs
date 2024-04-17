using RootMotion.FinalIK;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// This class will only be responsible for firing when OnTriggerCollider is fired
    /// On fired, it will return the respective interaction object for further processing
    /// </summary>
    public class TriggerBox : MonoBehaviour
    {
        public InteractableType type;

        [SerializeField] private Transform pivotPoint;
        [SerializeField] private Transform playerPosition;


        //TODO: Move to interactable object script
        //Interactable object details
        [SerializeField] private InteractionObject interactableObject;

        /// <summary>
        /// TODO: THIS WILL BE USED TO CONNECT WITH OTHER INTERACTION ELEMENTS OF THE GAME SYSTEM
        /// </summary>
        public InteractableObject interactionObject;
        public bool overridePlayerPosition;

        public Transform GetPivot() { return pivotPoint; }

        
        public Transform PlayerPosition { get {  return playerPosition; } }

        public InteractionObject GetInteractionObject() {  return interactableObject; }
    }
}
