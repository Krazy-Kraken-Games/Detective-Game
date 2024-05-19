using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Utility.DataStructures
{
    [System.Serializable]   
    public class TreeNode
    {
        public ConvoMessageSO messageData;
        public List<TreeNode> Children;
    }

    public class Tree
    {
        public TreeNode Node;

        public List<Tree> Children;

        public Tree(TreeNode _node)
        {
            Node = _node;

            Children = new List<Tree>();
        }

        public void AddChild(Tree child)
        {
            Children.Add(child);
        }
    }


}
