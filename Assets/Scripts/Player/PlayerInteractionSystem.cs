using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Managers;
using RootMotion.FinalIK;
using UnityEngine;


public class PlayerInteractionSystem : MonoBehaviour
{
    public static PlayerInteractionSystem instance = null;
    private InteractionSystem interactionSystem;

    public InteractionObject interactableObject;

    public FullBodyBipedEffector effector;

    private bool initiated;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        interactionSystem = GetComponent<InteractionSystem>();

       if (interactionSystem != null)
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
    }

    public void OpenDoorSequence()
    {
        if (initiated)
        {
            var completed = interactionSystem.StartInteraction(effector, interactableObject, true);

            if (completed)
            {
                //Logic to perform animations on door knob and the door itself

                //Then stopping door sequence once completed

                Invoke("StopInteractions", 1f);
            }
        }
    }

    public void StopInteractions()
    {
        interactionSystem.StopInteraction(effector);

        GamePlayerManager.instance.UpdateInputMode(MetaConstants.PlayerInputMode.PRIMARY);
    }
}
