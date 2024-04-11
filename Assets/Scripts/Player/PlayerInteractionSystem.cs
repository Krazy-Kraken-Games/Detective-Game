using RootMotion.FinalIK;
using UnityEngine;


public class PlayerInteractionSystem : MonoBehaviour
{
    public FullBodyBipedIK fullBodyIk;

    public InteractionSystem interactionSystem;

    public InteractionObject doorKnob;

    public FullBodyBipedEffector effector;

    private bool initiated;
    private void Start()
    {
       if(interactionSystem != null)
        {
            interactionSystem.OnInitiatedEvent += OnInteractionSystemInitiated;
        }
    }

    private void OnDestroy()
    {
        if (interactionSystem != null)
        {
            interactionSystem.OnInitiatedEvent -= OnInteractionSystemInitiated;
        }
    }

    private void OnInteractionSystemInitiated(bool hasInitiated)
    {
        initiated = hasInitiated;

        BeginInteraction();
    }

    private void BeginInteraction()
    {
        var completed = interactionSystem.StartInteraction(effector, doorKnob,true);

    }
}
