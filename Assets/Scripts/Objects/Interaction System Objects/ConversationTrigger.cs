using KrazyKrakenGames.DetectiveGame.Gameplay;
using KrazyKrakenGames.DetectiveGame.Global;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour
{
    [SerializeField] private ConversationInteraction conversationInteraction;

    [SerializeField] private bool showMultiple;
    [SerializeField] private bool hasShown = false;


    private void Start()
    {
        conversationInteraction = GetComponent<ConversationInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var collidedWith = other.gameObject;

        if (showMultiple)
        {
            if (collidedWith.tag == MetaConstants.PlayerTag)
            {
                Debug.Log("Start own thought convo");

                hasShown = true;
            }
        }
        else
        {
            if (hasShown) return;

            if (collidedWith.tag == MetaConstants.PlayerTag)
            {
                Debug.Log("Start own thought convo");

                hasShown = true;

                conversationInteraction.Interact();
            }
        }

        
    }
}
