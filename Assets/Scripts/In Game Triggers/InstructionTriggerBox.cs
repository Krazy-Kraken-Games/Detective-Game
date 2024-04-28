using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// An area trigger box that will trigger when player enters
    /// in this space for the first time
    /// </summary>
    public class InstructionTriggerBox : MonoBehaviour
    {
        [SerializeField] private bool hasTriggered = false;

        [Tooltip("Message to pop on instruction panel when area triggered")]
        [SerializeField] private string instructionMessage;

        private void OnTriggerStay(Collider other)
        {
            if(!hasTriggered)
            {
                if (other.gameObject.tag == "Player")
                {
                    hasTriggered = true;

                    UIManager.instance.ShowInstructionBox(instructionMessage);
                }
            }
        }
    }
}
