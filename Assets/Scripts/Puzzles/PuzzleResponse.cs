using System;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class PuzzleResponse : MonoBehaviour
    {
        private bool response;

        public bool Response => response;

        public Action<PuzzleResponse, bool> OnResponseRecievedEvent;

        public void SetResponse(bool _response)
        {
            response = _response;

            OnResponseRecievedEvent?.Invoke(this, response);
        }
    }
}