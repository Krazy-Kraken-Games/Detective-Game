using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class TriggerBox : MonoBehaviour
    {
        [SerializeField] private Transform pivotPoint;
        [SerializeField] private Transform playerPosition;

        public Transform GetPivot() { return pivotPoint; }

        public Transform PlayerPosition { get {  return playerPosition; } }
    }
}
