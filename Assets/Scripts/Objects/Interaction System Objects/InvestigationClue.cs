using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public enum InvestigationClueStatus
    {
        HIDDEN = 0,
        FOUND = 1
    }

    public class InvestigationClue : MonoBehaviour
    {
        [SerializeField] private InvestigationClueStatus status;

        public InvestigationClueStatus Status => status;

        public UnityEvent OnClueFoundEvent;

        public void ClueFound()
        {
            Debug.Log("Max value reached, clue solved");
            status = InvestigationClueStatus.FOUND;

            OnClueFoundEvent?.Invoke();
        }
    }
}
