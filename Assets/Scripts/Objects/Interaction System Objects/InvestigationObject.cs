using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public enum PuzzleState
    {
        UNSOLVED = 0,
        SOLVED = 1
    }

    public class InvestigationObject : MonoBehaviour
    {
        [SerializeField] private PuzzleState state;

        [SerializeField] private List<InvestigationClue> cluesToSolve;
        private List<InvestigationClue> cluesSolved;

        private void Start()
        {
            cluesSolved = new List<InvestigationClue>();
        }

        public void Add(InvestigationClue clue)
        {
            if(cluesSolved.Contains(clue)) return;

            cluesSolved.Add(clue);

            ValidateClues();
        }

        private void ValidateClues()
        {
            if(cluesSolved.Count == cluesToSolve.Count)
            {
                Debug.Log("Investigation complete");

                state = PuzzleState.SOLVED;
                UIManager.instance.AddToasterMessage("Investigation Complete");
            }
        }
    }
}
