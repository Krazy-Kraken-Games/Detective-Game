using KrazyKrakenGames.DetectiveGame.Global;
using System;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class PlacementCell : MonoBehaviour
    {
        [SerializeField] private PuzzleResponse puzzleResponse;

        [SerializeField] private PuzzlePiece correctResponse;

        [SerializeField] private bool hasResponse;
        public bool isCorrectPieceInPlace;

        [SerializeField] private MoveablePiece placedObject;

        private void Start()
        {
            puzzleResponse = GetComponent<PuzzleResponse>();    
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == MetaConstants.PieceTag)
            {
                if (!hasResponse)
                {
                    hasResponse = true;

                    placedObject = other.gameObject.GetComponent<MoveablePiece>();

                    placedObject.SetToPlacedState(this);

                    ValidateResponse();
                }
                else
                {
                    //If a response already exists, and another piece is being added to same 
                   
                    var tempPiece = other.gameObject.GetComponent<MoveablePiece>();
                    tempPiece.Reset();
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == MetaConstants.PieceTag && other.gameObject == placedObject)
            {
                PieceRemovedFromCell();
                ValidateResponse();
            }
        }

        public void PieceRemovedFromCell()
        {
            hasResponse = false;
            isCorrectPieceInPlace = false;
            placedObject = null;
        }

        private void ValidateResponse()
        {
            if (correctResponse != null)
            {
                if (placedObject == correctResponse)
                {
                    isCorrectPieceInPlace = true;
                    puzzleResponse.SetResponse(true);
                }
                else
                {
                    isCorrectPieceInPlace = false;
                    puzzleResponse.SetResponse(false);
                }
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} placement cell is missing correct response");
            }
        }
    }
}
