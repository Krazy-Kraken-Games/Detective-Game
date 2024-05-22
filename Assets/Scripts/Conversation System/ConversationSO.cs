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
    }
}
