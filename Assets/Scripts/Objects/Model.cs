using KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles;
using KrazyKrakenGames.DetectiveGame.Player;
using UnityEngine;


namespace KrazyKrakenGames.ThesisProject.GameModel
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private ActionController playerController;

        [SerializeField] private float rotationSpeed;

        [SerializeField] private InvestigationObject activeModel;

        private void Start()
        {
            if(playerController != null)
            {
                playerController.OnMoveInputEvent += OnRotateInputEventHandler;
            }

            activeModel = transform.GetChild(0).GetComponent<InvestigationObject>();
        }

        private void OnDestroy()
        {
            if (playerController != null)
            {
                playerController.OnMoveInputEvent -= OnRotateInputEventHandler;
            }
        }


        private void OnRotateInputEventHandler(Vector2 _rotation)
        {
            // Check which component of inputVector has greater magnitude
            if (Mathf.Abs(_rotation.x) > Mathf.Abs(_rotation.y))
            {
                // Rotate around y-axis (up in Unity) based on horizontal input
                transform.Rotate(Vector3.up, _rotation.x * rotationSpeed * Time.deltaTime,Space.World);
            }
            else
            {
                // Rotate around x-axis (right in Unity) based on vertical input
                transform.Rotate(Vector3.right, _rotation.y * rotationSpeed * Time.deltaTime,Space.World);
            }
        }

        public void SetNewActiveModel(InvestigationObject _object)
        {
            if(activeModel != null)
            {
                var temp = activeModel;
                activeModel = null;

                Destroy(temp.gameObject);
            }

            activeModel = Instantiate(_object, Vector3.zero, Quaternion.identity,transform);
            activeModel.transform.localPosition = Vector3.zero;
        }
    }
}
