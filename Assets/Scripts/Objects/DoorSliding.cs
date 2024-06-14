using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public class DoorSliding : MonoBehaviour, IObjectState
    {
        public ObjectState State { get; set; }

        [SerializeField] private Animator animator;

        private int ActivateKey;

        private void Start()
        {
            ActivateKey = Animator.StringToHash("Activate");
        }

        public void Close()
        {
            SetState(ObjectState.CLOSED);
        }

        public void Open()
        {
            if(State == ObjectState.OPEN)
            {
                UIManager.instance.AddToasterMessage("Door is already Open");
                return;
            }
            SetState(ObjectState.OPEN);
        }

        private void SetAnimatorBool(bool _value)
        {
            animator.SetBool(ActivateKey, _value);
        }

        public void SetState(ObjectState state)
        {
            State = state;

            bool flag = State == ObjectState.CLOSED ? false : true;
            SetAnimatorBool(flag);
        }
    }
}
