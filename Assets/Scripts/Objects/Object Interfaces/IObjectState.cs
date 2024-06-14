
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;
/// <summary>
/// This interface will refer to objects and their corresponding open and close states
/// For example: Is door open or close, Is window open or close
/// </summary>
public interface IObjectState
{
    public ObjectState State { get; set; }

    public void Close();
    public void Open();

    public void SetState(ObjectState state);
}
