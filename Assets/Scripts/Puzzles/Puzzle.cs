using System.Collections.Generic;
using UnityEngine;


namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class Puzzle : MonoBehaviour
    {
        public List<PlacementCell> placementCells = new List<PlacementCell>();
        public Dictionary<PlacementCell,bool> responses = new Dictionary<PlacementCell,bool>();

        private void Start()
        {
            foreach(var cell in placementCells)
            {
                cell.OnObjectPlacedEvent += OnPlacementCellUpdateHandler;
            }
        }

        private void OnPlacementCellUpdateHandler(PlacementCell cell, bool result)
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

            if(responses.Count == placementCells.Count)
            {
                //We got responses from all
                Debug.Log("Final validation check should happen now!");
                FinalValidationCheck();
            }
        }

        private void FinalValidationCheck()
        {
            foreach(var cell in placementCells)
            {
                if (!cell.isCorrectPieceInPlace)
                {
                    Debug.Log("One wrong check found. Puzzle unsolved");
                    return;
                }
            }

            Debug.Log("All checks completed! Puzzle solved");
        }
    }
}
