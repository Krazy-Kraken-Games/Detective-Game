using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Objects
{
    public class Door : MonoBehaviour
    {
        public enum State
        {
            OPEN = 0,
            LOCKED = 1
        }

        [SerializeField] private State currentState;

        private bool open;
        public bool direction = false;
        public float smooth = 2.0f;
        public float DoorOpenAngle = 90.0f;
        private float DoorNegAngle;
        private Quaternion defaultRot;
        private Quaternion openRot;

        public InteractableObject doorKnob;
        public InteractableObject negativeKnob; //To open in opposite direction


        public UnityEvent OnDoorOpenEvent;
        private void Start()
        {
            defaultRot = transform.rotation;
            DoorNegAngle = -DoorOpenAngle;


            doorKnob.OnInteractionInitEvent += OnInteractionDetected;

            if (negativeKnob != null)
            {
                negativeKnob.OnInteractionInitEvent += OnInteractionDetected;
            }

        }

        private void OnDestroy()
        {
            doorKnob.OnInteractionInitEvent -= OnInteractionDetected;

            if (negativeKnob != null)
            {
                negativeKnob.OnInteractionInitEvent -= OnInteractionDetected;
            }

        }

        private void Update()
        {
          if (open)
          {
                if (!direction)
                {
                    StartCoroutine(OpenDoor(DoorOpenAngle));
                }
                else
                {
                    StartCoroutine(OpenDoor(DoorNegAngle));
                }
             
          }
          else
          {
             StartCoroutine(CloseDoor());
          }
        }

        private IEnumerator OpenDoor(float openAngle)
        {
            //Find openRot angle
            openRot = Quaternion.Euler(defaultRot.eulerAngles + new Vector3(0, openAngle, 0));

            while (Quaternion.Angle(transform.rotation, openRot) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, openRot, Time.deltaTime * smooth);
                yield return null;
            }

            OnDoorOpenEvent?.Invoke();
        }

        private IEnumerator CloseDoor()
        {
            while (Quaternion.Angle(transform.rotation, defaultRot) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultRot, Time.deltaTime * smooth);
                yield return null;
            }
        }

        private void OnInteractionDetected(InteractableObject _object)
        {
           direction = _object.direction;   

            if (currentState == State.OPEN)
            {
                Invoke("UpdateDoorStatus", 1f);
            }
            else if (currentState == State.LOCKED)
            {
                UIManager.instance.AddToasterMessage("Door is locked");
            }
        }

        private void UpdateDoorStatus()
        {
            open = !open;
        }

        public void UnlockDoor()
        {
            Debug.Log("Door unlocked through questing");
            currentState = State.OPEN;
        }

        public void LockDoor()
        {
            currentState = State.LOCKED;

            StartCoroutine(CloseDoor());
        }
    }
}
