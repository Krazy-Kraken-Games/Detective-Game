using KrazyKrakenGames.DetectiveGame.Global;
using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections;
using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Objects
{
    public class Door : MonoBehaviour,IObjectState
    {
        private bool open;
        public float smooth = 2.0f;
        public float DoorOpenAngle = 90.0f;
        private Quaternion defaultRot;
        private Quaternion openRot;

        public InteractableObject doorKnob;

        public ObjectState State { get ; set ; }

        [Tooltip("Just a visual showcaser for local state")]
        [SerializeField] private ObjectState localState;

       
        private void Start()
        {
            defaultRot = transform.rotation;

            openRot = Quaternion.Euler(defaultRot.eulerAngles + new Vector3(0, DoorOpenAngle, 0));

            doorKnob.OnInteractionInitEvent += OnInteractionDetected;

        }

        private void OnDestroy()
        {
            doorKnob.OnInteractionInitEvent -= OnInteractionDetected;

        }

        private IEnumerator OpenDoor()
        {
            while (Quaternion.Angle(transform.rotation, openRot) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, openRot, Time.deltaTime * smooth);
                yield return null;
            }

            SetState(ObjectState.OPEN);
        }

        private IEnumerator CloseDoor()
        {
            while (Quaternion.Angle(transform.rotation, defaultRot) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultRot, Time.deltaTime * smooth);
                yield return null;
            }

            SetState(ObjectState.CLOSED);
        }

        private void OnInteractionDetected()
        {
            if (State == ObjectState.CLOSED)
            {
                Open();
            }
            else if (State == ObjectState.OPEN)
            {
                UIManager.instance.AddToasterMessage("Door is already open");
            }
        }

        public void UnlockDoor()
        {
            Debug.Log("Door unlocked through questing");
            SetState(ObjectState.OPEN);
        }

        public void Close()
        {
            StartCoroutine(CloseDoor());
        }

        public void Open()
        {
            StartCoroutine(OpenDoor());
        }

        public void SetState(ObjectState state)
        {
            State = state;
            localState = State;
        }
    }
}
