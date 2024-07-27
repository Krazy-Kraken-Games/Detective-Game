using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This script will attached on the player object ideally, and the functionality of this
/// script is limited to only teleporting a player from current position to targeted teleport point position
/// </summary>
public class TeleportSystem : MonoBehaviour
{
    public void Teleport(TeleportPoint tp)
    {
        //StartCoroutine(TeleportCoroutine(tp));
        TeleportPlayer(tp);
    }

    private async void TeleportPlayer(TeleportPoint tp)
    {
       bool success = await TeleportAsync(tp);

        if (success)
        {
            Debug.Log($"Teleportation Successful");
        }
        else
        {
            Debug.Log("Teleportation failure");
        }
    }

    private IEnumerator TeleportCoroutine(TeleportPoint tp)
    {
        yield return new WaitForEndOfFrame();

        transform.position = tp.teleportLocation;
    }

    public async Task<bool> TeleportAsync(TeleportPoint tp)
    {
        var tcs = new TaskCompletionSource<bool>();

        try
        {
            // Wait for the end of the frame
            await Task.Yield();

            // Set the position to the teleport location
            transform.position = tp.teleportLocation;

            tcs.SetResult(true);
        }
        catch(System.Exception ex)
        {
            tcs.SetException(ex);
        }

        return await tcs.Task;
    }
}
