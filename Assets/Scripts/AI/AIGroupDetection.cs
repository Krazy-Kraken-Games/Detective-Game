using KrazyKrakenGames.DetectiveGame.AI;
using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.AI
{
    /// <summary>
    /// This script will be responsible for organizing the attacks between 
    /// the AI crawlers. 
    /// Basic thought process is we do not want all enemies attacking the player 
    /// at once.
    /// </summary>

    public class AIGroupDetection : MonoBehaviour
    {
        [SerializeField] private List<Crawler> activeAgents = new List<Crawler>();

        private Queue<Crawler> attackers = new Queue<Crawler>();

        [SerializeField] private float lastAttackCounter;
        [SerializeField] private float maxAttackTimeCounter;

        private void Start()
        {
            lastAttackCounter = maxAttackTimeCounter;
        }

        private void Update()
        {
            if(attackers.Count > 0)
            {
                lastAttackCounter += Time.deltaTime;

                if(lastAttackCounter >= maxAttackTimeCounter )
                {
                    Debug.Log("Have someone attack");
                    RemoveAttacker();
                    lastAttackCounter = 0;
                }
            }
        }

        public void AddAgent(Crawler crawler)
        {
            if (activeAgents.Contains(crawler)) return;

            activeAgents.Add(crawler);
        }

        public void RemoveAgent(Crawler crawler)
        {
            if (!activeAgents.Contains(crawler)) return;

            activeAgents.Remove(crawler);
        }

        public void AddAttacker(Crawler crawler)
        {
            if (attackers.Contains(crawler)) return;

            attackers.Enqueue(crawler);
        }

        private void RemoveAttacker()
        {
            //var attacker =  attackers.Peek();

            var res = attackers.Dequeue();
            res.PerformAttack();
        }
    }
}
