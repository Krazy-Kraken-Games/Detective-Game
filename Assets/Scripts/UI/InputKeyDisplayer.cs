using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static KrazyKrakenGames.DetectiveGame.Global.GameControlConstants;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    [DefaultExecutionOrder(3)]
    /// <summary>
    /// This class will be responsible for displaying control key based on action to take
    /// </summary>
    public class InputKeyDisplayer : MonoBehaviour
    {
        [SerializeField] private ActionKey actionKey;

        [SerializeField] private TextMeshProUGUI label;

        private void Start()
        {
            SetupKey();
        }

        private void SetupKey()
        {
            if(PlayerControlsUI.instance != null)
            {
                KeyValuePair<string,InputAction> action = 
                    PlayerControlsUI.instance.allInputActions.First(pair => pair.Key == actionKey.ToString());

                var inputAction = action.Value;
                
                label.text = inputAction.GetBindingDisplayString();
            }
            else
            {
                Debug.LogWarning("PlayerControlsUI is not yet initiated");
            }
        }
    }
}
