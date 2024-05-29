using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ConversationNode : Node
{
    public string ID;
    public string Message;
    public List<ConversationNode> NextNodes;

    public ConversationNode ParentNode;

    public Port Input;


    public ConversationNode(string ID)
    {
        title = "Dialog Node";

        var label = new Label("Message");
        contentContainer.Add(label);

        var messageTextField = new TextField();
        messageTextField.RegisterValueChangedCallback(evt => Message = evt.newValue);
        contentContainer.Add(messageTextField);

        Input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        Input.portName = "Input";
        this.inputContainer.Add(Input);
        
    }

    public void SetParentNode(ConversationNode node)
    {
        ParentNode = node;
    }

    public void AddChildrenNodes(List<ConversationNode> nodes)
    {
        NextNodes = nodes;  
    }
}
