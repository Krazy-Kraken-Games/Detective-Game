using KrazyKrakenGames.DetectiveGame.Global;
using System;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class PlacementCell : MonoBehaviour
    {
        [SerializeField] private GameObject correctResponse;

        [SerializeField] private bool hasResponse;

        [SerializeField] private GameObject placedObject;

        public Action<bool> OnObjectPlacedEvent;

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == MetaConstants.PieceTag)
            {
                if (!hasResponse)
                {
                    hasResponse = true;

                    placedObject = other.gameObject;

                    ValidateResponse();
                }
                else
                {
                    //Send the puzzle piece back to its original position
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == MetaConstants.PieceTag)
            {
                hasResponse = false;

                placedObject = null;

                ValidateResponse();
            }
        }

        private void ValidateResponse()
        {
            if (correctResponse != null)
            {
                if (placedObject == correctResponse)
                {
                    OnObjectPlacedEvent?.Invoke(true);
                }
                else
                {
                    OnObjectPlacedEvent?.Invoke(false);
                }
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} placement cell is missing correct response");
            }
        }
    }
}
