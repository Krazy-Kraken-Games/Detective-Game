using System;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Utility
{
    public class Quest
    {
        private bool condition, outcome;

        public Action<bool> OnConditionUpdated;
        public Action<bool> OnOutcomeUpdated;
    }
}
