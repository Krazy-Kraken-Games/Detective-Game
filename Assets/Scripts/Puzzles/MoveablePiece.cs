using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Player;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles
{
    public class MoveablePiece : PuzzlePiece
    {
        private Vector3 defaultPosition;
        [SerializeField] private float YPostionOffset;
        [SerializeField] private float InAirYPosition;


        private Rigidbody rb;


        [SerializeField] private PlacementCell placedCell;

        private void Start()
        {
            defaultPosition = transform.position;

            rb = GetComponent<Rigidbody>();

            InAirYPosition = transform.position.y + YPostionOffset;
        }
        public override void Select(ActionController _actionController)
        {
            base.Select(_actionController);

            rb.isKinematic = true;

            transform.position =
                new Vector3(transform.position.x, InAirYPosition, transform.position.z);

            actionController.OnMoveInputEvent += Movement;

            if (placedCell != null)
            {
                //It was already lying on a placement cell
                placedCell.PieceRemovedFromCell();
                placedCell = null;
            }
        }

        public override void UnSelect(ActionController _actionController)
        {
            base.UnSelect(_actionController);

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

            this.transform.Translate(move * Time.deltaTime, Space.World);
        }


        public void SetToPlacedState(PlacementCell _cell)
        {
            state = State.PLACED;
            placedCell = _cell;
        }


        public void Reset()
        {
            transform.position = defaultPosition;
            state = State.UNSELECTED;
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
