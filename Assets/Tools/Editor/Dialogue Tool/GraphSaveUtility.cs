using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditor;

public class GraphSaveUtility : MonoBehaviour
{
    private DialogGraphView graphView;
    private DialogContainer cachedContainer;

    private List<Edge> Edges => graphView.edges.ToList();
    private List<DialogNode> Nodes => graphView.nodes.ToList().Cast<DialogNode>().ToList();
    public static GraphSaveUtility GetInstance(DialogGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            graphView = targetGraphView,
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return;

        var dialogContainer = ScriptableObject.CreateInstance<DialogContainer>();

        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for(var i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogNode;
            var inputNode = connectedPorts[i].input.node as DialogNode;

            dialogContainer.links.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.ID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.ID
            });
        }

        foreach(var dialogNode in Nodes.Where(node => !node.EntryPoint))
        {
            dialogContainer.nodes.Add(new DialogNodeData
            {
                ID = dialogNode.ID,
                Message = dialogNode.Message,
                Position = dialogNode.GetPosition().position
            }) ;
        }

        AssetDatabase.CreateAsset(dialogContainer, $"Assets/Resources/Conversations/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    public void LoadGraph(string fileName)
    {
        cachedContainer = Resources.Load<DialogContainer>(fileName);

        if(cachedContainer == null)
        {
            EditorUtility.DisplayDialog("File not found", "File doesnt exist in resources", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();


    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).ID = cachedContainer.nodes[0].ID;

        foreach(var node in Nodes)
        {
            if (node.EntryPoint) return;

            Edges.Where(x => x.input.node == node).ToList().
                ForEach(edge => graphView.RemoveElement(edge));

            graphView.RemoveElement(node);
        }
    }

    private void CreateNodes()
    {
        foreach(var node in cachedContainer.nodes)
        {
            var tempNode = graphView.CreateDialogNode(node.Message);
            tempNode.ID = node.ID;
            graphView.AddElement(tempNode);

            var nodePorts = cachedContainer.nodes.Where(x => x.ID == node.ID).ToList();


        }
    }
}
