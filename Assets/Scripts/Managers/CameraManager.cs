using Cinemachine;
using KrazyKrakenGames.DetectiveGame.Global;
using System;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance = null;

        private GamePlayerManager playerManager;

        [Header("Current Camera State")]
        [SerializeField] private GameCameraState State;

        [Space(5)]
        [Header("All Cameras used")]
        [SerializeField] private CinemachineVirtualCamera primaryCamera;
        [SerializeField] private Cinemachine3rdPersonFollow primaryFramingTransposer;

        [SerializeField] private CinemachineVirtualCamera secondaryCamera;

        public Action<GameCameraState> OnStateChangeEvent;

        public CinemachineVirtualCamera PrimaryCamera => primaryCamera;
        public Cinemachine3rdPersonFollow PrimaryFollowComponent => primaryFramingTransposer;

        public CinemachineVirtualCamera SecondaryCamera => secondaryCamera;

        public Vector3 middlePoint;
        #region UNITY_METHODS

        private void Awake()
        {
            if(instance == null )
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetState(GameCameraState.PRIMARY);

            playerManager = GamePlayerManager.instance;
            primaryFramingTransposer = primaryCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if (playerManager != null)
            {
                playerManager.OnGameStateChangedEvent += OnGameStateChangeEventHandler;
            }
        }

        private void OnDestroy()
        {
            if (playerManager != null)
            {
                playerManager.OnGameStateChangedEvent -= OnGameStateChangeEventHandler;
            }
        }

        private void LateUpdate()
        {
            middlePoint = GetMiddlePoint();

            //Debug.Log($"Middle Point in world Space: {middlePoint}");
        }

        #endregion

        #region STATE HANDLING

        public void SetState(GameCameraState state, Transform lookAt = null)
        {
            State = state;
            OnStateChangeEvent?.Invoke(state);

            OnStateChange(state, lookAt);
        }

        public GameCameraState GetState()
        {
            return State;
        }


        private void OnStateChange(GameCameraState state, Transform lookAt = null)
        {
            switch (state)
            {
                case GameCameraState.PRIMARY:
                    primaryCamera.gameObject.SetActive(true);
                    secondaryCamera.gameObject.SetActive(false);
                    break;

                case GameCameraState.SECONDARY:
                    primaryCamera.gameObject.SetActive(false);
                    secondaryCamera.gameObject.SetActive(true);

                    if(lookAt != null)
                    {
                        Debug.Log("We have a pivot point");
                        secondaryCamera.m_LookAt = lookAt;
                    }
                    break;
            }
        }

        private void OnGameStateChangeEventHandler(GameState _state)
        {
            if (State == GameCameraState.PRIMARY)
            {
                if (_state == GameState.SHOOT)
                {
                    PlayerShootCameraMode();
                }
                else
                {
                    PlayerFollowCameraMode();
                }
            }
        }

        #endregion

        #region PRIMARY CAMERA UPDATE HANDLING

        private void PlayerShootCameraMode()
        {

            primaryFramingTransposer.CameraDistance = GameCameraConstants.ShootCameraDistance;
            primaryFramingTransposer.ShoulderOffset = GameCameraConstants.ShootCameraShoulderOffset;
        }

        private void PlayerFollowCameraMode()
        {
            primaryFramingTransposer.CameraDistance = GameCameraConstants.FollowCameraDistance;
            primaryFramingTransposer.ShoulderOffset = GameCameraConstants.FollowCameraShoulderOffset;
        }

        #endregion


        private Vector3 GetMiddlePoint()
        {
            Camera cam = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;

            // Get the middle point of the screen in screen space (normalized)
            Vector3 screenMiddlePoint = new Vector3(0.5f, 0.5f, cam.nearClipPlane);

            // Convert the screen point to a world point
            Vector3 worldMiddlePoint = cam.ViewportToWorldPoint(screenMiddlePoint);

            return worldMiddlePoint;

        }
    }
}
