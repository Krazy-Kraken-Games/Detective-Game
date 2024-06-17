using KrazyKrakenGames.DetectiveGame.Gameplay;
using KrazyKrakenGames.DetectiveGame.Global;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private InteractableObject interaction;

    [SerializeField] private bool showMultiple;
    [SerializeField] private bool hasShown = false;


    private void Start()
    {
        interaction = GetComponent<InteractableObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var collidedWith = other.gameObject;

        if (showMultiple)
        {
            if (collidedWith.tag == MetaConstants.PlayerTag)
            {
                hasShown = true;

                interaction.Interact();
            }
        }
        else
        {
            if (hasShown) return;

            if (collidedWith.tag == MetaConstants.PlayerTag)
            {
                hasShown = true;

                interaction.Interact();
            }
        }

        
    }
}
