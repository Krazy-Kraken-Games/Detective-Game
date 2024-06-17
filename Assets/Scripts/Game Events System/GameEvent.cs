using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// A Game Event is defined as a executable living breathing entity in game
    /// which contains information about what could occur when it is executed
    /// Basic example of a game event are: Narrative Trigger, Cinematic Scene Played, A Conversation Triggered
    /// </summary>
    public class GameEvent : MonoBehaviour,IGameEvent,IGameInputEvent
    {
        //Two types of effects would be triggered on an activity with the game event
        public UnityEvent OnTriggeredEffect;
        public UnityEvent OnExecutedEffect;

        public bool EventCompleted = false;

        [Space(5)]
        [Header("Input Action to complete event")]
        [SerializeField] private InputAction requiredAction;
        [SerializeField] private bool ListenToInput = false;

        [SerializeField] private InputActionMap actionMap;
        [SerializeField] private InputActionAsset allActions;

        [SerializeField] private InputAction newAction;
        public InputAction IA 
        { 
            set
            {
                if(requiredAction != null)
                {
                    requiredAction.performed -= InputListenerCallback;
                    requiredAction.Disable();
                    requiredAction.Dispose();
                }

                requiredAction = value;
            } 
            get => requiredAction; 
        }
        public void OnEventTriggered()
        {
            OnTriggeredEffect?.Invoke();
            RegisterListener();
        }

        public void OnEventExecuted()
        {
            OnExecutedEffect?.Invoke();
            EventCompleted = true;
        }

        public void RegisterListener()
        {
            IA.performed += InputListenerCallback;
            IA.Enable();

            ListenToInput = true;
        }

        public void UnregisterListener()
        {
            IA.performed -= InputListenerCallback;
            ListenToInput = false;

            IA.Disable();
            IA.Dispose();
        }

        private void InputListenerCallback(CallbackContext context)
        {
            UnregisterListener();
            

            Debug.Log("Action completed! Unregistering the game event listener");
            OnEventExecuted();
        }

        private void Start()
        {
            FindNewAction("Player","Interact");
        }

        private void FindNewAction(string actionMapName, string actionName)
        {
            Debug.Log("Findign actions");
            var actionMap = allActions.FindActionMap(actionMapName);
            if (actionMap != null)
            {
                newAction = actionMap.FindAction(actionName);
                if (newAction == null)
                {
                    Debug.LogError($"Action '{actionName}' not found in Action Map '{actionMapName}'");
                }
                else
                {
                    Debug.Log("Actioon found");
                }
            }
            else
            {
                Debug.LogError($"Action Map '{actionMapName}' not found in Input Action Asset");
            }
        }
    }
}
