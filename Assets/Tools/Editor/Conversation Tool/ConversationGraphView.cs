using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ConversationGraphView : GraphView
{
    private List<ConversationNode> copiedNodes = new List<ConversationNode>();

    public ConversationGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(new ContextualMenuManipulator(BuildContextMenu));

        var gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);

        //Add root element
        AddElement(CreateRootNode());

        this.graphViewChanged = GraphViewChanged;

    }

    #region Context Menu Manipulation

    private void BuildContextMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Create Node", action => CreateNodeWithPosition("Dialog Node", action.eventInfo.mousePosition));
        evt.menu.AppendAction("Copy Node", action => CopyNode(), ShouldBeCopied);
        evt.menu.AppendAction("Paste Node", action => PasteNodes(action.eventInfo.mousePosition), ShouldBePasted);
        
    
    }

    private bool CanCopy()
    {
        return selection.Count > 0;
    }

    private DropdownMenuAction.Status ShouldBeCopied(DropdownMenuAction _action)
    {
        return CanCopy() ? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Disabled;

    }

    private void CopyNode()
    {
        copiedNodes.Clear();

        foreach(var node in selection)
        {
            if(node is ConversationNode selectedNode)
            {
                copiedNodes.Add(selectedNode);
            }
        }
    }


    private bool CanPaste()
    {
        return copiedNodes.Count > 0;
    }

    private DropdownMenuAction.Status ShouldBePasted(DropdownMenuAction _action)
    {
        return CanPaste() ? DropdownMenuAction.Status.Normal: DropdownMenuAction.Status.Disabled;
    }

    private void PasteNodes(Vector2 mousePosition)
    {
        float inBetweenNodeWidth = 350;

        for(int i = 0; i < copiedNodes.Count; i++)
        {
            Vector2 nextPosition = new Vector2(mousePosition.x + (i * inBetweenNodeWidth),mousePosition.y);

            var createdNode = CreateNodeWithPosition("Pasted Node", nextPosition);

            
            createdNode.MessageTextField.value = copiedNodes[i].Message;

            createdNode.TypeEnumField.value = copiedNodes[i].Type;
        }
    }

    #endregion

    #region Node Creation
    public ConversationNode CreateRootNode()
    {
        var Node = new ConversationNode(Guid.NewGuid().ToString());

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

    private ConversationNode CreateNodeWithPosition(string _message, Vector2 position)
    {
        var node = new ConversationNode(Guid.NewGuid().ToString());

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(position.x, position.y, 300, 150));

        Debug.Log($"Node Created at: {position}");

        AddElement(node);

        return node;
    }

    #endregion

    #region Graph Change
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
    #endregion

    #region Port Connections

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

        private void OnPortConnected(Edge edge)
    {
        var inputNode = edge.input.node as ConversationNode;
        var outputNode = edge.output.node as ConversationNode;

        if (inputNode != null && outputNode != null)
        {
            Debug.Log($"Connected {outputNode.Message} to {inputNode.Message}");

            if(nodes.Contains(inputNode) && nodes.Contains(outputNode))
            {
                var _inNode = nodes.Where(x => x == inputNode).First() as ConversationNode;
                var _outNode = nodes.Where(x => x == outputNode).First() as ConversationNode;
                _inNode.SetParentNode(_outNode);

                _outNode.AddChildren(_inNode);
            }
        }
    }

    private void OnPortDisconnected(Edge edge)
    {
        var inputNode = edge.input.node as ConversationNode;
        var outputNode = edge.output.node as ConversationNode;

        if (inputNode != null && outputNode != null)
        {
            Debug.Log($"Disconnected {outputNode.title} from {inputNode.title}");

            if (nodes.Contains(inputNode) && nodes.Contains(outputNode))
            {
                var _inNode = nodes.Where(x => x == inputNode).First() as ConversationNode;
                var _outNode = nodes.Where(x => x == outputNode).First() as ConversationNode;

                _inNode.SetParentNode(null);

                _outNode.RemoveChild(_inNode);
            }
        }
    }

    #endregion
}
