using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// The responsiblity of this script is to run on narration instance
    /// and give the game an ability to run cinematic/cut scenes, trigger events in game
    /// </summary>
    [DefaultExecutionOrder(6)]
    public class NarrativeInteraction : InteractableObject
    {
        //A Game Event will be executed on interaction with this
        public override void Interact()
        {
            base.Interact();

            Debug.Log("Narrative execution in process!");
        }
    }
}
