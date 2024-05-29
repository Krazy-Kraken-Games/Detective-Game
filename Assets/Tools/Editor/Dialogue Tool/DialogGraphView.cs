using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
    private Vector2 defaultNodeSize = new Vector2(200, 150);

    public DialogGraphView()
    {

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0,gridBackground );

        AddElement(GenerateEntryPointNode());
    }

    private DialogNode GenerateEntryPointNode()
    {
        var node = new DialogNode
        {
            title = "START",
            ID = Guid.NewGuid().ToString(),
            Message = "Hello World",
            EntryPoint = true
        };

        var generatePort = GeneratePort(node, Direction.Output);
        generatePort.portName = "Output";

        node.outputContainer.Add(generatePort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));

        return node;
    }

    private Port GeneratePort(DialogNode _node, Direction _portDirection,Port.Capacity _capacity = Port.Capacity.Single)
    {
        return _node.InstantiatePort(Orientation.Horizontal, _portDirection, _capacity,typeof(float));
    }

    public DialogNode CreateDialogNode(string nodeName)
    {
        var dialogNode = new DialogNode
        {
            title = nodeName,
            ID = Guid.NewGuid().ToString(),
            Message = "Hello World"
        };

        var inputPort = GeneratePort(dialogNode, Direction.Input,Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogNode.inputContainer.Add(inputPort);

        var button = new Button(() =>
        {
            AddChoicePort(dialogNode);
        });
        dialogNode.titleContainer.Add(button);
        button.text = "New choice";

        dialogNode.RefreshExpandedState();
        dialogNode.RefreshPorts();
        dialogNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
       

        return dialogNode;
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogNode(nodeName));
    }


    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if(startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });
        
        return compatiblePorts;
    }

    public void AddChoicePort(DialogNode node,string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(node, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = node.outputContainer.Query("connector").ToList().Count;

        var outputPortName = $"Choice {outputPortCount}";

        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {outputPortCount + 1}"
            : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
       
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(textField);

        var deleteButton = new Button(() => RemovePort(node, generatedPort))
        {
            text = "X"
        };

        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    private void RemovePort(DialogNode _node, Port _port)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == _port.portName && x.output.node == _port.node);

        if (!targetEdge.Any()) return;

        var edge = targetEdge.First();
        edge.input.Disconnect(edge);

        RemoveElement(edge);

        _node.outputContainer.Remove(_port);
        _node.RefreshExpandedState();
        _node.RefreshPorts();
    }
}
