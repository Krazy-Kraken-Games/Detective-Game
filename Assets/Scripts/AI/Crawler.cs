using KrazyKrakenGames.DetectiveGame.Gameplay.Shooting;
using System;
using System.Collections.Generic;
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


        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private int currentWaypointIndex;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            OnStateChangedEvent += OnLocalStateChangedHandler;

            SetState(EnemyState.IDLE);

            SetDestination(waypoints[0].position);
        }

        private void OnDestroy()
        {
            OnStateChangedEvent -= OnLocalStateChangedHandler;
        }

        private void Update()
        {
            WaypointSystem();
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

            agent.SetDestination(targetDestination);

            SetState(EnemyState.PATROL);

        }
        #endregion


        #region Waypoint System Section

        private Vector3 NextWaypointPosition()
        {
           
            Debug.Log($"Go to: {waypoints[currentWaypointIndex].position}");
            return waypoints[currentWaypointIndex].position;
        }

        private void TraverseToNextWaypoint()
        {
            currentWaypointIndex++;

            if(currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }

            Vector3 targetPos = NextWaypointPosition();

            SetDestination(targetPos);
        }

        private void WaypointSystem()
        {
            if(Vector3.Distance(targetDestination,transform.position) < 1f)
            {
                //Switch to next target
                TraverseToNextWaypoint();
            }
        }

        #endregion

    }
}
