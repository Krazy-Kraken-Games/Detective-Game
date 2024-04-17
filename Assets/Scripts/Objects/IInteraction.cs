using UnityEngine;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;

/// <summary>
/// This interface will determine what kind of interaction was instigated by the player
/// on the object
/// </summary>
public interface IInteraction
{
   public InteractableType type { get;}

    public void Interact();
}
