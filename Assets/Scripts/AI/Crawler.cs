using KrazyKrakenGames.DetectiveGame.Gameplay.Shooting;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace KrazyKrakenGames.DetectiveGame.AI
{
    public enum EnemyState
    {
        IDLE = 0,
        PATROL = 1,
        FOLLOW = 2,
        ATTACK = 3,
        DEATH = 4
    }

    /// <summary>
    /// This script will hold the AI behavior of Spirit Crawler
    /// </summary>
    public class Crawler : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private NavMeshPath currentPath;
        [SerializeField] private Vector3 targetDestination;

        [SerializeField] private EnemyState state;
        public EnemyState State => state;

        public Action<EnemyState> OnStateChangedEvent;


        [SerializeField] private LineRenderer pathRenderer;
        public int currentPathIndex = 0;
        //TO BE REMOVED:
        [SerializeField] private Transform targetLocation;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            OnStateChangedEvent += OnLocalStateChangedHandler;

            SetState(EnemyState.IDLE);

            if(targetLocation != null)
            SetDestination(targetLocation.position);
        }

        private void OnDestroy()
        {
            OnStateChangedEvent -= OnLocalStateChangedHandler;
        }

        private void Update()
        {
            if (currentPath != null)
            {

                AgentRotation(agent.nextPosition);
            }
        }

        public void SetState(EnemyState _state)
        {
            state = _state;
            OnStateChangedEvent?.Invoke(state);
        }

        private void OnLocalStateChangedHandler(EnemyState _newState)
        {
            if(_newState == EnemyState.DEATH)
            {
                Debug.Log("Destroying myself");

                Destroy(gameObject);
            }
        }

        #region Collision Detection Section

        public void OnCollisionWithPlayer()
        {
            //Destroy immedietaly for now
            SetState(EnemyState.DEATH);
        }

        public void OnCollisionWithProjectile(Projectile projectile)
        {
            //Just destroy for now
            SetState(EnemyState.DEATH);
        }

        #endregion

        #region AI Navigation Section

        private void SetDestination(Vector3 _targetLocation)
        {
            targetDestination = _targetLocation;
            NavMeshPath path = agent.path;
           
            agent.CalculatePath(_targetLocation, path);

            currentPath = path;
            

            agent.path = path;

            pathRenderer.positionCount = agent.path.corners.Length;

            pathRenderer.SetPosition(0, transform.position);
            pathRenderer.SetPositions(agent.path.corners);

            agent.SetDestination(targetDestination);

        }
        #endregion


        private void AgentRotation(Vector3 _nextPosition)
        {
            //Vector3 direction = targetDestination - transform.position;

            //Quaternion rotation = Quaternion.LookRotation(direction);
            

            //transform.rotation = rotation;

            //
        }

        Vector3 GetNextPosition(NavMeshAgent agent)
        {
            NavMeshPath path = agent.path;
            if (path.corners.Length > 1)
            {
                // Return the next corner in the path
                return path.corners[1];
            }
            else
            {
                // If there are no corners, return the agent's current position
                return agent.transform.position;
            }
        }
    }
}
