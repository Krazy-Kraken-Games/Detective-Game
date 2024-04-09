using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.PostProcessingConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class DetectiveMode : MonoBehaviour
    {
        [Header("Post Processing Manager and References")]
        [SerializeField] private PostProcessingManager postProcessManager;

        [Space(5)]
        [Header("Monochromatic Sphere Variables and References")]
        [SerializeField] private GameObject monoSphere;
        [SerializeField] private float defaultScale;
        [SerializeField] private float maxScale;
        [SerializeField] private float sphereScaleTime;  //Time taken by the sphere to reach max size/scale

        private float currentActiveTime; //Time detective mode is active for
        private float maxActiveTime; //Max time detective mode can stay active for

        [SerializeField] private bool isDetectiveModeActive;
        public Action OnDetectiveModeProcessEvent;
        public Action OnDetectiveModeActivated;
        public Action OnDetectiveModeDeactivated;

        private Coroutine activeCoroutine;

        private void Start()
        {
            postProcessManager = PostProcessingManager.Instance;

            sphereScaleTime = MetaConstants.TimeToActivate;
            maxActiveTime = MetaConstants.DetectiveModeEndTime;

            isDetectiveModeActive = false;
            currentActiveTime = 0;
            
            OnDetectiveModeActivated += OnDetectiveModeActivatedHandler;
            OnDetectiveModeDeactivated += OnDetectiveModeDeactivatedHandler;
        }

        private void OnDestroy()
        {
            OnDetectiveModeActivated -= OnDetectiveModeActivatedHandler;
            OnDetectiveModeDeactivated -= OnDetectiveModeDeactivatedHandler;
        }

        private void Update()
        {
            if (!isDetectiveModeActive) return;

            if(currentActiveTime < maxActiveTime)
            {
                currentActiveTime += Time.deltaTime;
            }
            else
            {
                isDetectiveModeActive = false;
                EndDetectiveMode();
              
            }
        }

        private void OnDetectiveModeActivatedHandler()
        {
            isDetectiveModeActive = true;
        }

        private void OnDetectiveModeDeactivatedHandler()
        {

        }

        public void StartDetectiveMode()
        {
            ActivateDetectiveModeLogic();
        }

        public void EndDetectiveMode()
        {
            if(activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);

                if (monoSphere.activeInHierarchy)
                {
                    ResetMonoSphere();
                }
            }

            if (postProcessManager != null)
            {
                postProcessManager.DeactiveVolume();
                OnDetectiveModeDeactivated?.Invoke();

                currentActiveTime = 0;
            }
            else
            {
                Debug.LogWarning("Post Processing Manager not found", gameObject);
            }
        }

        #region Post Process Manager Handling and Callbacks

        private void ActivateDetectiveModePostProcess()
        {
            if(postProcessManager != null )
            {

                if(activeCoroutine != null)
                {
                    activeCoroutine = null;
                }
                postProcessManager.ActivateVolume(ProcessVolume.DETECTIVE);

                OnDetectiveModeActivated?.Invoke();

                ResetMonoSphere();
            }
            else
            {
                Debug.LogWarning("Post Processing Manager not found", gameObject);
            }
        }

        #endregion


        #region Monochromatic Sphere Manipulation

        private void ActivateDetectiveModeLogic()
        {
            monoSphere.SetActive(true);

            activeCoroutine = StartCoroutine(IncreaseSphereSizeAsync());

            OnDetectiveModeProcessEvent?.Invoke();
        }

        private IEnumerator IncreaseSphereSizeAsync()
        {
            float startTime = Time.time;

            while(Time.time - startTime < sphereScaleTime)
            {
                float t = (Time.time - startTime)/ sphereScaleTime;

                float amountToIncrease = Mathf.Lerp(1f, maxScale, t);
                monoSphere.transform.localScale = new Vector3(amountToIncrease, amountToIncrease, amountToIncrease);

                yield return null;
            }
            ActivateDetectiveModePostProcess();
        }

        private void ResetMonoSphere()
        {
            monoSphere.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
            monoSphere.SetActive(false);
        }

        #endregion
    }
}
