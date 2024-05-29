using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;

public class ConversationGraphView : GraphView
{
    public ConversationGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);

        //Add root element
        AddElement(CreateRootNode());

        this.graphViewChanged = GraphViewChanged;

    }

    private Port GeneratePort(ConversationNode _node, Direction _portDirection, Port.Capacity _capacity = Port.Capacity.Single)
    {
        return _node.InstantiatePort(Orientation.Horizontal, _portDirection, _capacity, typeof(float));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public ConversationNode CreateRootNode()
    {
        var Node = new ConversationNode(Guid.NewGuid().ToString());

        var generatePort = GeneratePort(Node, Direction.Output);
        generatePort.portName = "Output";
        Node.outputContainer.Add(generatePort);


        Node.RefreshExpandedState();
        Node.RefreshPorts();
        Node.SetPosition(new Rect(100, 100, 300, 150));
        return Node;
        
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateConvoNode(nodeName));
    }

    private ConversationNode CreateConvoNode(string _message)
    {
        var Node = new ConversationNode(Guid.NewGuid().ToString());

        Node.RefreshExpandedState();
        Node.RefreshPorts();

        Node.SetPosition(new Rect(100, 0, 300, 150));

        return Node;
    }


    private GraphViewChange GraphViewChanged(GraphViewChange graphViewChange)
    {
        Debug.Log("Something new is added!");
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                OnPortConnected(edge);
            }
        }

        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var element in graphViewChange.elementsToRemove)
            {
                if (element is Edge edge)
                {
                    OnPortDisconnected(edge);
                }
            }
        }

        return graphViewChange;
    }

    private void OnPortConnected(Edge edge)
    {
        var inputNode = edge.input.node as ConversationNode;
        var outputNode = edge.output.node as ConversationNode;

        if (inputNode != null && outputNode != null)
        {
            Debug.Log($"Connected {outputNode.Message} to {inputNode.Message}");

            inputNode.SetParentNode(outputNode);
        }
    }

    private void OnPortDisconnected(Edge edge)
    {
        var inputNode = edge.input.node as ConversationNode;
        var outputNode = edge.output.node as ConversationNode;

        if (inputNode != null && outputNode != null)
        {
            Debug.Log($"Disconnected {outputNode.title} from {inputNode.title}");

            inputNode.SetParentNode(null);
        }
    }
}
