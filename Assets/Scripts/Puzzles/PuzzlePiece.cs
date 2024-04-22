using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class PuzzlePiece : MonoBehaviour
    {
        public enum State
        {
            UNSELECTED = 0,
            SELECTED = 1
        }

        private Vector3 defaultPosition;
        [SerializeField] private float onSelectYPosition;

        [SerializeField] private bool isSelected;

        [SerializeField] private State state;

        private void Start()
        {
            defaultPosition = transform.position;
        }

        private void Update()
        {
            if (!isSelected) return;
        }
        public void Select()
        {
            state = State.SELECTED;
            isSelected = true;

            transform.position = 
                new Vector3(defaultPosition.x,onSelectYPosition,defaultPosition.z);
        }

        public void UnSelect()
        {
            state = State.UNSELECTED;

            isSelected = false;

            transform.position = defaultPosition;
        }

        
    }
}
