using KrazyKrakenGames.ThesisProject.InputHandling;
using UnityEngine;


namespace KrazyKrakenGames.ThesisProject.GameModel
{
    public class Model : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;

        [SerializeField] private float rotationSpeed;

        [SerializeField] private GameObject uiInfoPanel;

        private void Start()
        {
            if(playerController != null)
            {
                playerController.OnRotateModelInput += OnRotateInputEventHandler;
                playerController.OnShowModelInfoInput += OnShowInfoInputHandler;
            }
        }

        private void OnDestroy()
        {
            if (playerController != null)
            {
                playerController.OnRotateModelInput -= OnRotateInputEventHandler;
                playerController.OnShowModelInfoInput -= OnShowInfoInputHandler;
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

        private void OnShowInfoInputHandler(bool _showInfo)
        {
            uiInfoPanel.SetActive(_showInfo);
        }
    }
}
