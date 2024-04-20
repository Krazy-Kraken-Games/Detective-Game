using KrazyKrakenGames.DetectiveGame.Global;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(-1)]
    public class PlayerControlsUI : MonoBehaviour
    {
        public static PlayerControlsUI instance = null; 
        public PlayerInput _input;

        public Dictionary<string, InputAction> allInputActions = new Dictionary<string, InputAction>();

        private void Awake()
        {
            if(instance == null)
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
            DisplayAllActions();
        }

        private void DisplayAllActions()
        {
            if (_input != null)
            {
                var allActions = _input.currentActionMap.actions;

                foreach (InputAction action in allActions)
                {
                    if (string.Equals(action.name, GameControlConstants.Cancel))
                    {
                        //We only want to deal with Constant right now
                        allInputActions.Add(action.name, action);
                    }
                }
            }
        }
    }
}
