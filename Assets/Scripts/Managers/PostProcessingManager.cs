using UnityEngine;
using UnityEngine.Rendering;
using static KrazyKrakenGames.DetectiveGame.Global.PostProcessingConstants;

namespace KrazyKrakenGames.DetectiveGame.Managers
{
    public class PostProcessingManager : MonoBehaviour
    {
        public static PostProcessingManager Instance = null;
        private Volume activatedVolume;

        [Header("Scene References to Global Volumes")]
        [SerializeField] private Volume detectiveModeVolume;


        #region Unity Methods

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Volume Activation and Deactivation section

        /// <summary>
        /// Activate Post Process Volume based on Volume Code
        /// </summary>
        /// <param name="_volumeCode">Provided volume code to activate</param>
        public void ActivateVolume(ProcessVolume _volumeCode)
        {
            Volume volume = GetVolume(_volumeCode);

            if(volume != null)
            {
                volume.weight = 1;
                activatedVolume = volume;
            }
            else
            {
                Debug.LogWarning($"Post Processing volume with code: {_volumeCode} not found");
            }
        }


        /// <summary>
        /// Deactive active post process volume
        /// </summary>
        public void DeactiveVolume()
        {
            if (activatedVolume != null)
            {
                activatedVolume.weight = 0;
                activatedVolume = null;
            }
            else
            {
                Debug.LogWarning("No active post process volume could be found");
            }
           
        }

        /// <summary>
        /// Deactive post process volume by code
        /// </summary>
        /// <param name="_volumeCode">Provided volume code to deactivate</param>
        public void DeactiveVolumeByCode(ProcessVolume _volumeCode)
        {
            Volume volume = GetVolume(_volumeCode);

            if (volume != null)
            {
                volume.weight = 0;
                activatedVolume = null;
            }
        }

        #endregion

        #region Utility/Helper Functions
        /// <summary>
        /// Helper function to get volume based on enum volume code
        /// </summary>
        /// <param name="_volumeCode"></param>
        /// <returns></returns>
        private Volume GetVolume(ProcessVolume _volumeCode)
        {
            switch(_volumeCode) 
            {

                case ProcessVolume.DETECTIVE:
                    return detectiveModeVolume;
            }

            return null;
        }

#endregion
    }
}
