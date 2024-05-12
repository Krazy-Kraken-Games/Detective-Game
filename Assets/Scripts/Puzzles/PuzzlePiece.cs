using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Player;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class PuzzlePiece : MonoBehaviour
    {
        public enum State
        {
            UNSELECTED = 0,
            SELECTED = 1,
            PLACED = 2 //Placed on puzzle
        }
        [SerializeField] private bool isSelected;

        [SerializeField] protected State state;

        public virtual void Select(ActionController _actionController)
        {
            state = State.SELECTED;
            isSelected = true;
        }

        public virtual void UnSelect(ActionController _actionController)
        {
            state = State.UNSELECTED;

            isSelected = false;
        }
    }
}
