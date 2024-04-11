using RootMotion.FinalIK;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class TriggerBox : MonoBehaviour
    {
        public InteractableType type;

        [SerializeField] private Transform pivotPoint;
        [SerializeField] private Transform playerPosition;


        //Interactable object details
        [SerializeField] private InteractionObject interactableObject;

        public Transform GetPivot() { return pivotPoint; }

        public Transform PlayerPosition { get {  return playerPosition; } }

        public InteractionObject GetInteractionObject() {  return interactableObject; }
    }
}
