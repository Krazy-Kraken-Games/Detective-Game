using UnityEngine.InputSystem;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public interface IGameInputEvent
    {
        InputAction IA { set; get; }

        void RegisterListener();
        void UnregisterListener();
    }
}
