using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class TriggerBox : MonoBehaviour
    {
        [SerializeField] private Transform pivotPoint;

        public Transform GetPivot() { return pivotPoint; }
    }
}
