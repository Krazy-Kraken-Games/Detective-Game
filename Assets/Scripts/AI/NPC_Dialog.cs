using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.AI
{
    /// <summary>
    /// Temporary Dialog animation controller for NPC script, will be updated and maybe removed in future
    /// </summary>
    public class NPC_Dialog : MonoBehaviour
    {
        private Animator animator;


        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void StartConversation()
        {
            animator.SetBool("Conversation", true);
        }

        public void EndConversation()
        {
            animator.SetBool("Conversation", false);
        }
    }
}
