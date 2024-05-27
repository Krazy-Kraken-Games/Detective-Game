using System;
using UnityEngine;

public class PlayerDetectionTrigger : MonoBehaviour
{
    [SerializeField] private bool readyForUse = true;
    [SerializeField] private SphereCollider sphereCollider;

    public Action<GameObject> OnPlayerDetectedEvent;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (readyForUse)
        {
            var collidedWith = other.gameObject;
            if (collidedWith.tag == "Player")
            {
                OnPlayerDetectedEvent?.Invoke(collidedWith);

                readyForUse = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (readyForUse)
        {
            var collidedWith = other.gameObject;
            if (collidedWith.tag == "Player")
            {
                OnPlayerDetectedEvent?.Invoke(collidedWith);

                readyForUse = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var collidedWith = other.gameObject;
        if (collidedWith.tag == "Player")
        {
            readyForUse = true;
        }
    }
}
