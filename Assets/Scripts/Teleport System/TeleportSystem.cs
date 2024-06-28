using UnityEngine;

/// <summary>
/// This script will attached on the player object ideally, and the functionality of this
/// script is limited to only teleporting a player from current position to targeted teleport point position
/// </summary>
public class TeleportSystem : MonoBehaviour
{
    public void Teleport(TeleportPoint tp)
    {
        transform.position = tp.teleportLocation;
    }
}
