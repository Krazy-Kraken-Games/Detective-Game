using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KrazyKrakenGames.DetectiveGame.QuestSystem
{
    [Tooltip("Attach this script to every object that needs to be included in quest's KILL segment")]
    public class QuestTarget : MonoBehaviour
    {
        public UnityEvent OnObjectKilled;

        public void KillSelf()
        {
            OnObjectKilled?.Invoke();

            Destroy(gameObject);
        }
    }
}
