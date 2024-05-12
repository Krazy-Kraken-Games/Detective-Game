using KrazyKrakenGames.DetectiveGame.UI;
using System.Collections;
using UnityEngine;

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
        public float smooth = 2.0f;
        public float DoorOpenAngle = 90.0f;
        private Quaternion defaultRot;
        private Quaternion openRot;

        public InteractableObject doorKnob;


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

        private void Update()
        {
          if (open)
          {
             StartCoroutine(OpenDoor());
          }
          else
          {
             StartCoroutine(CloseDoor());
          }
        }

        private IEnumerator OpenDoor()
        {
            while (Quaternion.Angle(transform.rotation, openRot) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, openRot, Time.deltaTime * smooth);
                yield return null;
            }
        }

        private IEnumerator CloseDoor()
        {
            while (Quaternion.Angle(transform.rotation, defaultRot) > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultRot, Time.deltaTime * smooth);
                yield return null;
            }
        }

        private void OnInteractionDetected()
        {
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
    }
}
