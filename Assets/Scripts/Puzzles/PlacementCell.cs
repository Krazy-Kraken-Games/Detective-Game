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

        [SerializeField] private PuzzlePiece placedObject;

        public Action<PlacementCell,bool> OnObjectPlacedEvent;


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

                    placedObject = other.gameObject.GetComponent<PuzzlePiece>();

                    placedObject.SetToPlacedState(this);

                    ValidateResponse();
                }
                else
                {
                    //If a response already exists, and another piece is being added to same 
                    Debug.Log("Send puzzle piece back");
                    var tempPiece = other.gameObject.GetComponent<PuzzlePiece>();
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
                    Debug.Log("Correct object placed");
                    isCorrectPieceInPlace = true;
                    //OnObjectPlacedEvent?.Invoke(this,true);
                    puzzleResponse.SetResponse(true);
                }
                else
                {
                    Debug.Log("Wrong object placed");
                    isCorrectPieceInPlace = false;
                    //OnObjectPlacedEvent?.Invoke(this,false);
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
