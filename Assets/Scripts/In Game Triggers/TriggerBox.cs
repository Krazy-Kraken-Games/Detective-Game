using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// This class will only be responsible for firing when OnTriggerCollider is fired
    /// On fired, it will return the respective interaction object for further processing
    /// </summary>
    public class TriggerBox : MonoBehaviour
    {
        [SerializeField] private Transform pivotPoint;
        [SerializeField] private Transform playerPosition;

        [SerializeField] private GameObject uiVisualizer;

        /// <summary>
        /// Holds reference to the interactable object partner
        /// </summary>
        public InteractableObject interactionObject;
        public bool overridePlayerPosition;

        public Transform GetPivot() { return pivotPoint; }

        
        public Transform PlayerPosition { get {  return playerPosition; } }

        public void ShowUIBox()
        {
            if(uiVisualizer != null)
            {
                uiVisualizer.SetActive(true);
            }
        }

        public void HideUIBox()
        {

            if (uiVisualizer != null)
            {
                uiVisualizer.SetActive(false);
            }
        }

    }
}
