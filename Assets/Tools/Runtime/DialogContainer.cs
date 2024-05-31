using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogContainer : ScriptableObject
{
    public List<NodeLinkData> links = new List<NodeLinkData>();
    public List<DialogNodeData> nodes = new List<DialogNodeData>();
}
