using KrazyKrakenGames.DetectiveGame.Global;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour
{
    [SerializeField] private bool showMultiple;
    [SerializeField] private bool hasShown = false;
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
            }
        }

        
    }
}
