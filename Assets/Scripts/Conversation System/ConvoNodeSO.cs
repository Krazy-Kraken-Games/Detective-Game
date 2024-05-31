using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ConvoNode", menuName = "Conversation System/Convo Node")]
public class ConvoNodeSO : ScriptableObject
{
    public string NodeID;
    public string id;
    public ConvoMessageSO messageData;
    public List<ConvoNodeSO> Children;

    public ConvoNodeSO ParentNode;
}
