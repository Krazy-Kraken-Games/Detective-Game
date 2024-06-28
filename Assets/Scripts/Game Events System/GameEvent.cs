using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static KrazyKrakenGames.DetectiveGame.Global.GameControlConstants;
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
        public bool Listening = false;
        public bool EventCompleted = false;

        //Two types of effects would be triggered on an activity with the game event
        public UnityEvent OnTriggeredEffect;
        public UnityEvent OnExecutedEffect;

        [Space(5)]
        [Header("Input Action to complete event")]
        [SerializeField] private ActionKey buttonKey;

        private InputActionMap actionMap;
        [SerializeField] private InputActionAsset allActions;

        private InputAction newAction;

        
        public InputAction IA 
        { 
            set
            {
                if(newAction != null)
                {
                    newAction.performed -= InputListenerCallback;
                    newAction.Disable();
                    newAction.Dispose();
                }

                newAction = value;
            } 
            get => newAction; 
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
            Listening = false;
        }

        public void RegisterListener()
        {
            IA.performed += InputListenerCallback;
            Listening = true;
        }

        public void UnregisterListener()
        {
            IA.performed -= InputListenerCallback;
        }

        private void InputListenerCallback(CallbackContext context)
        {
            UnregisterListener();
            

            Debug.Log("Action completed! Unregistering the game event listener");
            OnEventExecuted();
        }

        private void Start()
        {
            FindNewAction("Player", buttonKey.ToString());
        }

        private InputAction FindNewAction(string actionMapName, string actionName)
        {
            var actionMap = allActions.FindActionMap(actionMapName);
            if (actionMap != null)
            {
                newAction = actionMap.FindAction(actionName);
                if (newAction == null)
                {
                    return null;
                }
                else
                {
                    return newAction;
                }
            }
            else
            {
                Debug.LogError($"Action Map '{actionMapName}' not found in Input Action Asset");
                return null;
            }
        }
    }
}
