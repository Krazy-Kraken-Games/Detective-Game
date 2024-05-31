using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Conversations
{
    [CreateAssetMenu(fileName = "Conversation Object", menuName = "Conversation System/ ConversationSO")]
    public class ConversationSO : ScriptableObject
    {
        [SerializeField] private List<ConvoNodeSO> nodes;

        public List<ConvoNodeSO> Nodes => nodes;

        //Jump nodes
        public int questInProgressIndex;
        public int questCompleteIndex;

        public void SetNodes(List<ConvoNodeSO> _nodes)
        {
            this.nodes = _nodes;
        }

        public void AddNode(ConvoNodeSO _node)
        {
            if(nodes == null)
            {
                nodes = new List<ConvoNodeSO>();
            }

            if (nodes.Contains(_node)) return;

            nodes.Add(_node);
        }
    }
}
