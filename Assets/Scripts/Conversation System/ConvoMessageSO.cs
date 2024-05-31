using KrazyKrakenGames.DetectiveGame.Gameplay;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ConvoMessage",menuName = "Conversation System/Convo Message")]
public class ConvoMessageSO : ScriptableObject
{
    public string speakerName;  //Later we change it to be character
    public string message;

    //Check if it has associated quest with it
    public DialogMessageType MessageType;

    public int QuestSegmentID; //Check with message type, and populate message accordingly

}
