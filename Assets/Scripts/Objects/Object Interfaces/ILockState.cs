
using System;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;
/// <summary>
/// This interface will be used on all items that have a lock and unlock state
/// Best case examples: Door, Lockers, Drawers,Phones?
/// </summary>
public interface ILockState
{
    public LockState State { get; set; }

    public Action<LockState> OnStateChange { get; set; }
    public void Unlock();
    public void Lock();

    public void SetState(LockState _state);
}
