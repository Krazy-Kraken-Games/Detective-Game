using System;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.UI
{
    public enum ScreenState
    {
        Screen1 = 0,
        Screen2 = 1, 
        Screen3 = 2,
        Screen4 = 3
    }

    public class UIScreenSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Screens = new List<GameObject>();
        private ScreenState currentState;
        [SerializeField] private ScreenState DefaultScreen;

        public Action<ScreenState> OnScreenStateChanged;

        private void Start()
        {
            SetState(DefaultScreen);
        }

        public void SetState(ScreenState _newState)
        {
            currentState = _newState;
            OnScreenStateChanged?.Invoke(currentState);

            UpdateScreen(currentState);
        }

        private void UpdateScreen(ScreenState _newState)
        {
            int index = (int)_newState;

            for(int i =0; i < Screens.Count; i++)
            {
                bool show = Screens.IndexOf(Screens[i]) == index ? true : false;

                Screens[i].SetActive(show);
            }
        }
    }
}
