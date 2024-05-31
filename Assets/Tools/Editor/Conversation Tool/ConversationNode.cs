using KrazyKrakenGames.DetectiveGame.Gameplay;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ConversationNode : Node
{
    public string NodeID;
    public string ID;
    public string Message;
    public DialogMessageType Type;
    public List<ConversationNode> NextNodes;

    public ConversationNode ParentNode;

    public Port Input;
    public Port Output;


    public ConversationNode(string _ID)
    {
        title = "Dialog Node";
        ID = _ID;

        var idLabel = new Label("Node ID");
        contentContainer.Add(idLabel);

        var nodeIdInputField = new TextField();
        //nodeIdInputField.RegisterValueChangedCallback(evt => NodeID = evt.newValue);
        nodeIdInputField.RegisterValueChangedCallback(OnIDValueChanged);
        contentContainer.Add(nodeIdInputField);

        var label = new Label("Message");
        contentContainer.Add(label);

        var messageTextField = new TextField();
        messageTextField.RegisterValueChangedCallback(evt => Message = evt.newValue);
        contentContainer.Add(messageTextField);

        var typeLabel = new Label("Type");
        mainContainer.Add(typeLabel);

        var enumTypeField = new EnumField("Message Type", Type);
        enumTypeField.RegisterValueChangedCallback(OnTypeFieldValueChanged);
        mainContainer.Add(enumTypeField);

        Input = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        Input.portName = "Input";
        this.inputContainer.Add(Input);

        Output = Port.Create<Edge>(Orientation.Horizontal, Direction.Output,Port.Capacity.Multi, typeof(float));
        Output.portName = "Output";
        this.outputContainer.Add(Output);
        
    }

    public void SetParentNode(ConversationNode node)
    {
        ParentNode = node;

        if (node != null)
        {
            Debug.Log($"{Message} setting parent node to: {node.Message}");
        }

        
    }

    public void AddChildrenNodes(List<ConversationNode> nodes)
    {
        NextNodes = nodes;  
    }

    public void AddChildren(ConversationNode _node)
    {
        if(NextNodes == null)
        {
            NextNodes = new List<ConversationNode>();
        }

        if(!NextNodes.Contains(_node))
        {
            NextNodes.Add(_node);
        }
    }

    public void RemoveChild(ConversationNode node)
    {
        if (NextNodes == null) return;

        if(NextNodes.Contains(node))
        {
            NextNodes.Remove(node);
        }
    }

    private void OnTypeFieldValueChanged(ChangeEvent<Enum> evt)
    {
        Type = (DialogMessageType)evt.newValue;
    }

    private void OnIDValueChanged(ChangeEvent<string> evt)
    {
        NodeID = evt.newValue;
        title = evt.newValue.ToString();
    }
}
