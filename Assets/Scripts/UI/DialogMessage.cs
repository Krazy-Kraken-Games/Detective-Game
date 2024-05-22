
namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    public enum MessageType
    {
        DEFAULT = 0,
        QUESTGIVER = 1,
        QUESTACTIVE = 2,
        QUESTENDED = 3, //After firing the given segment ends
        ENDER = 4
    }

    [System.Serializable]
    public class DialogMessage
    {
        public string Message;
        public int MessageID;

        //Check if it has associated quest with it
        public MessageType MessageType;
        public int QuestID;
        public int QuestSegmentID; //Check with message type, and populate message accordingly

    }
}
