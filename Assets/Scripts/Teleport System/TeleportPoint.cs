using UnityEngine;

/// <summary>
/// This script will only be responsible for denoting where the object should teleport to
/// 
/// </summary>
public class TeleportPoint : MonoBehaviour
{
    [SerializeField] private Transform locationPoint;

    public Vector3 teleportLocation => locationPoint.position;
}
