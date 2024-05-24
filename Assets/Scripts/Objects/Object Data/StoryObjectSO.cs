using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay
{
    /// <summary>
    /// This scriptable object will hold data about objects in game
    /// </summary>
    /// 

    [CreateAssetMenu(fileName = "StoryObject",menuName = "Detective Game/Narrative/Story Object")]
    public class StoryObjectSO : ScriptableObject
    {
        public string ObjectName;
    }
}
