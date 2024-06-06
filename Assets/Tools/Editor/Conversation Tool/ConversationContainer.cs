using KrazyKrakenGames.DetectiveGame.Conversations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ConversationContainer : MonoBehaviour
{
    private ConversationGraphView graphView;

    private Dictionary<string,ConversationNode> cachedConvoNodes = new Dictionary<string, ConversationNode>();
    
    private List<Edge> Edges => graphView.edges.ToList();
    private List<ConversationNode> Nodes => graphView.nodes.ToList().Cast<ConversationNode>().ToList();
    public static ConversationContainer GetInstance(ConversationGraphView targetGraphView)
    {
        return new ConversationContainer
        {
            graphView = targetGraphView,
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) 
        {
            EditorUtility.DisplayDialog("Invalid Graph", "Graph Doesnt Have Any Edges", "Okay");
            return;
        }

        ConversationSO conversationSO;
        string convoPath = $"Conversations/{fileName}";

        string filePath = $"Assets/Resources/Conversations/{fileName}";

        CheckIfFolderExists(filePath);

        if (CheckIfConvoAssetExists(convoPath))
        {
            conversationSO = Resources.Load(convoPath) as ConversationSO;
        }
        else
        {
            conversationSO = new ConversationSO();
            AssetDatabase.CreateAsset(conversationSO, $"Assets/Resources/Conversations/{fileName}/{fileName}.asset");
        }
        cachedConvoNodes.Clear();

        //Check or create related Messages and Node Folder

        //Message Folder
        string messageFilePath = $"Assets/Resources/Conversations/{fileName}/Messages";
        CheckIfFolderExists(messageFilePath);

        //Nodes Folder
        string nodeFilePath = $"Assets/Resources/Conversations/{fileName}/Nodes";
        CheckIfFolderExists(nodeFilePath);


        //For loop to create all nodes and messages
        foreach (var node in Nodes)
        {
            ConvoMessageSO messageSO;
            ConvoNodeSO nodeSO;
            string messagePath = $"Conversations/{fileName}/Messages/{node.NodeID}";
            string nodePath = $"Conversations/{fileName}/Nodes/{node.NodeID}";

            if (CheckIfMessageAssetExists(messagePath))
            {
                messageSO = Resources.Load<ConvoMessageSO>(messagePath);
                messageSO.message = node.Message;
                messageSO.speakerName = node.title;
                messageSO.MessageType = node.Type;
            }
            else
            {
                messageSO = new ConvoMessageSO();
                messageSO.message = node.Message;
                messageSO.speakerName = node.title;
                messageSO.MessageType = node.Type;

                AssetDatabase.CreateAsset(messageSO, $"Assets/Resources/Conversations/{fileName}/Messages/{node.NodeID}.asset");
            }

            if (CheckIfAssetExists(nodePath))
            {
                nodeSO = Resources.Load<ConvoNodeSO>(nodePath);

                nodeSO.NodeID = node.NodeID;
                nodeSO.id = node.ID;
                nodeSO.name = node.name;
                nodeSO.messageData = messageSO;
                nodeSO.ParentNode = null;

                if (nodeSO.Children != null && nodeSO.Children.Count > 0)
                {
                    nodeSO.Children.Clear();
                }

                var parentNode = GetParentNode(node, graphView) as ConversationNode;

                if (parentNode != null)
                {
                    node.ParentNode = parentNode;
                }

                if (cachedConvoNodes.ContainsKey(nodeSO.NodeID))
                {
                    cachedConvoNodes.Add(nodeSO.NodeID, node);
                }
                else
                {
                    cachedConvoNodes[nodeSO.NodeID] = node;
                }
            }
            else
            {
                nodeSO = new ConvoNodeSO();
                nodeSO.NodeID = node.NodeID;
                nodeSO.id = node.ID;
                nodeSO.name = node.name;
                nodeSO.messageData = messageSO;

                var parentNode = GetParentNode(node, graphView) as ConversationNode;

                if (parentNode != null)
                {
                    node.ParentNode = parentNode;
                }

                if (cachedConvoNodes.ContainsKey(nodeSO.NodeID))
                {
                    cachedConvoNodes.Add(nodeSO.NodeID, node);
                }
                else
                {
                    cachedConvoNodes[nodeSO.NodeID] = node;
                }


                AssetDatabase.CreateAsset(nodeSO, $"Assets/Resources/Conversations/{fileName}/Nodes/{nodeSO.NodeID}.asset");

            }

            conversationSO.AddNode(nodeSO);

            AssetDatabase.SaveAssets();
        }

        foreach (var node in Nodes)
        {
            //Check if previous nodes are its parents

            Debug.Log($"cachedNode count: {cachedConvoNodes.Count}");
            if (cachedConvoNodes.Count > 0 && cachedConvoNodes != null)
            {
                //Sets the parent Node
                if (node.ParentNode != null)
                {
                    if (cachedConvoNodes.ContainsKey(node.ParentNode.NodeID))
                    {
                        ConvoNodeSO parentNode = Resources.Load<ConvoNodeSO>($"Conversations/{fileName}/Nodes/{node.ParentNode.NodeID}");
                        if (parentNode != null)
                        {
                            //Parent Node SO has been created

                            ConvoNodeSO asset = Resources.Load<ConvoNodeSO>($"Conversations/{fileName}/Nodes/{node.NodeID}");
                            asset.ParentNode = parentNode;
                        }
                    }
                }


                //Sets the children Node

                if (node.NextNodes != null) 
                {
                    if (node.NextNodes.Count > 0)
                    {
                        foreach (var child in node.NextNodes)
                        {
                            if (cachedConvoNodes.ContainsKey(child.NodeID))
                            {
                                ConvoNodeSO childNode = Resources.Load<ConvoNodeSO>($"Conversations/{fileName}/Nodes/{child.NodeID}");

                                if (childNode != null)
                                {
                                    //Child node has been created

                                    ConvoNodeSO asset = Resources.Load<ConvoNodeSO>($"Conversations/{fileName}/Nodes/{node.NodeID}");

                                    if (asset.Children == null)
                                    {
                                        asset.Children = new List<ConvoNodeSO>();
                                    }

                                    if (!asset.Children.Contains(childNode))
                                        asset.Children.Add(childNode);
                                }
                            }
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }

    }

    public static Node GetParentNode(Node targetNode, GraphView graphView)
    {
        // Iterate through all edges in the graph view
        foreach (var edge in graphView.edges.ToList())
        {
            // Check if the edge's input is the target node
            if (edge.input.node == targetNode)
            {
                // Return the node at the edge's output
                return edge.output.node as Node;
            }
        }
        // Return null if no parent node is found
        return null;
    }

    private bool CheckIfAssetExists(string path)
    {
        var Node = Resources.Load<ConvoNodeSO>(path);

        if(Node == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Helper function to check if folder exists at filepath. Create a folder if it doesnt
    /// </summary>
    /// <param name="filePath"></param>
    private void CheckIfFolderExists(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            Debug.Log("Folder path exists");
        }
        else
        {
            Debug.Log("Folder path doesnt exist");

            Directory.CreateDirectory(filePath);

            Debug.Log("Folder path now created. Please check");
        }
    }

    private bool CheckIfMessageAssetExists(string path)
    {
        var Message = Resources.Load<ConvoMessageSO>(path);

        if(Message == null) { return false; }

        return true;
    }

    private bool CheckIfConvoAssetExists(string path)
    {
        var Convo = Resources.Load<ConversationSO>(path);

        if (Convo == null) { return false; }

        return true;
    }
}
