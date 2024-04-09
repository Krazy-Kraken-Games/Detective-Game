using Cinemachine;
using System;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager instance = null;

        [Header("Current Camera State")]
        [SerializeField] private GameCameraState State;

        [Space(5)]
        [Header("All Cameras used")]
        [SerializeField] private CinemachineVirtualCamera primaryCamera;
        [SerializeField] private CinemachineVirtualCamera secondaryCamera;

        public Action<GameCameraState> OnStateChangeEvent;

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
        #endregion
    }
}
