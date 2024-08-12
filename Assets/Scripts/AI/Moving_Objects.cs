using UnityEngine;

public class Moving_Objects : MonoBehaviour
{
    [SerializeField] private Transform modelBody;

    //Two points
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Transform endPosition;

    [SerializeField] private bool ToEnd;

    [Range(3f, 40f)]
    [SerializeField] private float Speed;

    [SerializeField] private float distanceThreshold;

    private void Start()
    {
        startPosition = transform.position;
        modelBody = transform.GetChild(0);
    }

    private void Update()
    {
        if (ToEnd)
        {
            GoToTarget(endPosition.position);

            if(Vector3.Distance(endPosition.position, transform.position) < distanceThreshold)
            {
                ToEnd = false;
            }
        }
        else
        {
            GoToTarget(startPosition);

            if(Vector3.Distance(startPosition, transform.position) < distanceThreshold)
            {
                ToEnd = true;
            }
        }
    }

    private void GoToTarget(Vector3 _targetPosition)
    {
        Vector3 direction = _targetPosition - transform.position;
        direction = direction.normalized;
        transform.Translate(direction * Speed * Time.deltaTime);

        Quaternion rotation = Quaternion.LookRotation(direction);
        modelBody.rotation = rotation;
    }
}
