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

        private Vector3 defaultPosition;
        [SerializeField] private float onSelectYPosition;

        [SerializeField] private bool isSelected;

        [SerializeField] private State state;

        private Rigidbody rb;

        private ActionController actionController;

        [SerializeField] private PlacementCell placedCell;

        private void Start()
        {
            defaultPosition = transform.position;

            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!isSelected) return;
        }
        public void Select(ActionController _actionController)
        {
            state = State.SELECTED;
            isSelected = true;
            rb.isKinematic = true;

            transform.position = 
                new Vector3(transform.position.x,onSelectYPosition, transform.position.z);

            actionController = _actionController;
            _actionController.OnMoveInputEvent += Movement;

            if(placedCell != null)
            {
                //It was already lying on a placement cell
                placedCell.PieceRemovedFromCell();
                placedCell = null;
            }
        }

        public void UnSelect(ActionController _actionController)
        {
            state = State.UNSELECTED;

            isSelected = false;
            rb.isKinematic = false;

            if (actionController != null)
            {
                _actionController.OnMoveInputEvent -= Movement;
            }

            actionController = null;
        }

        private void Movement(Vector2 movement)
        {
            Vector3 move = new Vector3(movement.x, 0f, movement.y);

            // Normalize the direction to ensure consistent speed in all directions
            move.Normalize();

            this.transform.Translate(move * Time.deltaTime);
        }

        public void Reset()
        {
            transform.position = defaultPosition;
            state = State.UNSELECTED;
        }

        public void SetToPlacedState(PlacementCell _cell)
        {
            state = State.PLACED;
            placedCell = _cell;
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (state == State.PLACED) return;

            if (collision.gameObject.tag == MetaConstants.PieceTag)
            {
                Reset();
            }
        }
    }
}
