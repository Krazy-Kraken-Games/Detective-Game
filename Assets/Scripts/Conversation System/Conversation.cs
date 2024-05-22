using KrazyKrakenGames.DetectiveGame.Conversations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ConvoNode", menuName = "Conversation System/Convo Node")]
public class ConvoNodeSO : ScriptableObject
{
    public string id;
    public ConvoMessageSO messageData;
    public List<ConvoNodeSO> Children;

    public ConvoNodeSO ParentNode;
}

[System.Serializable]
public class SiblingGroup
{
    public ConvoNodeSO Parent;
    public List<ConvoNodeSO> Siblings;

    public SiblingGroup(List<ConvoNodeSO> _siblingList, ConvoNodeSO _parent)
    {
        Parent = _parent;
        Siblings = _siblingList;
    }
}

public class Conversation : MonoBehaviour
{
    public ConversationSO conversationSO;
    public List<ConvoNodeSO> allConversationNodes;

    public ConvoNodeSO rootNode;
    public int Levels;  //Refers to number of tiers the tree has

    public int currentLevel;
    public ConvoNodeSO processingParentNode;

    public Dictionary<ConvoNodeSO,SiblingGroup> allConvoSiblings = new Dictionary<ConvoNodeSO,SiblingGroup>();
    public SiblingGroup currentGroup;

    public ConvoNodeSO previousConvoNode;
    public ConvoNodeSO activeConvoNode;
    [Space(10)]
    [Header("UI Related")]
    public ConvoUI convoPrefab;
    public Transform parent;

    private void Start()
    {
        allConversationNodes = conversationSO.Nodes;
        StartTraversing();
    }

    public async void StartTraversing()
    {
        currentLevel = 0;
       
        var root = allConversationNodes.Where(item => item.ParentNode == null).First();
        rootNode = root;
        processingParentNode = rootNode;
        Debug.Log($"Root node found: {rootNode.name}");

        await SiblingFinder();

        Debug.Log("All nodes processed");

        previousConvoNode = rootNode;
        activeConvoNode = rootNode;
        currentGroup = allConvoSiblings[activeConvoNode];
        PopulateNode(activeConvoNode);
    }

    private async Task SiblingFinder()
    {
        var siblingList = allConversationNodes.Where(item => item.ParentNode == processingParentNode).ToList();

        if (siblingList.Count > 0)
        {
            SiblingGroup sibGroup = new SiblingGroup(siblingList, processingParentNode);
            allConvoSiblings.Add(processingParentNode,sibGroup);
            //currentLevel++;

            foreach (var sibling in siblingList)
            {
                processingParentNode = sibling;

                await SiblingFinder();
            }
        }
        else
        {
            return;
        }
    }

    private void PopulateNode(ConvoNodeSO _node)
    {
        activeConvoNode = _node;
        ConvoUI go = Instantiate(convoPrefab, parent);

        go.Populate(_node,this);
    }

    public void ShowChildren(ConvoNodeSO convoNode)
    {
        
        DestroyAllCurrentMessages();

        foreach(var sibling in convoNode.Children)
        {
            PopulateNode(sibling);
        }
    }

    public void BackToActiveTree()
    {
        if (activeConvoNode.ParentNode.ParentNode != null)
        {
            ShowChildren(activeConvoNode.ParentNode.ParentNode);
        }
        else
        {
            Debug.Log("No path back, end the conversation");
        }
    }
    

    public void DestroyAllCurrentMessages()
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
