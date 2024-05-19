using UnityEngine;

[CreateAssetMenu(fileName = "ConvoMessage",menuName = "Conversation System/Convo Message")]
public class ConvoMessageSO : ScriptableObject
{
    public string speakerName;  //Later we change it to be character
    public string message;
}
