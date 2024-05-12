using KrazyKrakenGames.DetectiveGame.Player;
using TMPro;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class NumberPiece : PuzzlePiece
    {
        public int correctResponse;

        [SerializeField] private PuzzleResponse puzzleResponse;

        [SerializeField] private int currentNumber = 0;

        [SerializeField] private TextMeshProUGUI valueText;

        private int minLimit = 0;
        private int maxLimit = 9;

        private float valueChangeThreshold = 0.125f; //Allows the value to be changed after every threshold seconds/frames
        private bool allowChange = true;

        private void Start()
        {
            puzzleResponse = GetComponent<PuzzleResponse>();
        }

        public override void Select(ActionController _actionController)
        {
            base.Select(_actionController);

            actionController.OnMoveInputEvent += CycleThrough;
        }

        public override void UnSelect(ActionController _actionController)
        {
            base.UnSelect(_actionController);


            if (actionController != null)
            {
                _actionController.OnMoveInputEvent -= CycleThrough;
            }

            actionController = null;
        }

        private void CycleThrough(Vector2 movement)
        {
            //For now, just use movement updater
            if (!allowChange) return;
            if(movement == Vector2.zero) return;

            if(movement.y != 0)
            {
                if(movement.y < 0)
                {
                    if(currentNumber < maxLimit)
                    {
                        //Increase value
                        currentNumber += 1;
                    }
                    else
                    {
                        //Reset
                        currentNumber = minLimit;
                    }

                }
                else if(movement.y > 0)
                {
                    if (currentNumber > minLimit)
                    {
                        //Decrease value
                        currentNumber -= 1;
                    }
                    else
                    {
                        //Reset
                        currentNumber = maxLimit;
                    }
                }

                allowChange = false;
                valueText.text = $"{currentNumber}";

                ValidateResponse();
                Invoke("AllowInputChange", valueChangeThreshold);
            }
        }

        private void AllowInputChange()
        {
            allowChange = true;
        }

        private void ValidateResponse()
        {
            if(currentNumber != correctResponse)
            {
                puzzleResponse.SetResponse(false);
            }
            else if(currentNumber == correctResponse)
            {
                puzzleResponse.SetResponse(true);
            }
        }
    }
}
