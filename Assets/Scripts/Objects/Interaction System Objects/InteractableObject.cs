using RootMotion.FinalIK;
using System;
using UnityEngine;
using UnityEngine.Events;
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

        //Root IK component reference
        [SerializeField] private InteractionObject interactableObject;

        public UnityEvent OnInteractedWith;

        //Only being used for door, refactor later to remove this variable
        public bool direction;
        public InteractableType type
        {
            get
            {
                return Type;
            }
        }

        public Action<InteractableObject> OnInteractionInitEvent;

        private void Start()
        {
            interactableObject = GetComponent<InteractionObject>();
        }

        public virtual void Interact()
        {
            PlayerInteractionSystem.instance.interactableObject = interactableObject;
            PlayerInteractionSystem.instance.StartInteraction();

            OnInteractionInitEvent?.Invoke(this);
            OnInteractedWith?.Invoke();
        }
    }
}
