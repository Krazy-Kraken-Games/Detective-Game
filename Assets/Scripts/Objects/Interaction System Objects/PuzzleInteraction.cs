using KrazyKrakenGames.DetectiveGame.Managers;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class PuzzleInteraction : InteractableObject
    {
        [SerializeField] private Transform lookAt;
        public override void Interact()
        {
            Debug.Log("Puzzle interaction through inheritance");

            CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
            GamePlayerManager.instance.UpdateInputMode(PlayerInputMode.SECONDARY);
            GamePlayerManager.instance.UpdateMode(GameState.PUZZLE);
        }
    }
}
