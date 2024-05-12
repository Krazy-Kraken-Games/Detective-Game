using System.Collections.Generic;
using UnityEngine;


namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class Puzzle : MonoBehaviour
    {
        public List<PuzzleResponse> puzzleResponseReferences = new List<PuzzleResponse>();
        public Dictionary<PuzzleResponse, bool> responses = new Dictionary<PuzzleResponse, bool>();

        [SerializeField] private Renderer notifierRenderer;

        [SerializeField] private Material puzzleSolvedMaterial;
        [SerializeField] private Material puzzleUnsolvedMaterial;

        private void Start()
        {
            foreach(var puzzleResponser in puzzleResponseReferences)
            {
                puzzleResponser.OnResponseRecievedEvent += OnResponseUpdateHandler;
            }

            notifierRenderer.material = puzzleUnsolvedMaterial;
        }

        private void OnResponseUpdateHandler(PuzzleResponse cell, bool result)
        {
            //Update happened on this cell
            if (responses.ContainsKey(cell))
            {
                responses[cell] = result;
            }
            else
            {
                responses.Add(cell, result);
            }

            if(responses.Count == puzzleResponseReferences.Count)
            {
                //We got responses from all
                Debug.Log("Final validation check should happen now!");
                FinalValidationCheck();
            }
        }

        private void FinalValidationCheck()
        {
            foreach(var puzzleResponser in puzzleResponseReferences)
            {
                if (!puzzleResponser.Response)
                {
                    Debug.Log("One wrong check found. Puzzle unsolved");
                    notifierRenderer.material = puzzleUnsolvedMaterial;
                    return;
                }
            }

            Debug.Log("All checks completed! Puzzle solved");
            notifierRenderer.material = puzzleSolvedMaterial;
        }
    }
}
