using System;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Objects
{
    public class KeyCard : InteractableObject, ILockState
    {
        public LockState State {get;set;}

        public Action<LockState> OnStateChange { get; set; }

        [SerializeField] private DoorSliding door;

        public void Lock()
        {
            SetState(LockState.LOCKED);
        }

        public void Unlock()
        {
            SetState(LockState.UNLOCKED);

            door.Open();
        }

        public void SetState(LockState _state)
        {
            State = _state;
            OnStateChange?.Invoke(State);
        }

        public override void Interact()
        {
            ///<summary>
            ///Pseudo Code:
            ///Check what state the object is: 
            ///If locked then give a message locked
            ///If unlocked, then it can be opened
            /// </summary>
            /// 

            var isLocked = IsLocked();

            if (isLocked)
            {
                CanBeUnlocked();
            }
            else
            {
                door.Open();
            }

            base.Interact();
            Debug.Log($"Interacting! Change to Unlock State: {isLocked}");
        }

        private bool IsLocked()
        {
            return State == LockState.LOCKED;
        }

        /// <summary>
        /// Perform the check to see if the door can be unlocked
        /// If yes then unlock it
        /// </summary>
        private void CanBeUnlocked()
        {
            //For now, we just unlock it
            //TODO: In future, we want to check with inventory, to see if we got the item to unlock (KEY)

            Invoke("Unlock",0.5f);
        }
    }
}
