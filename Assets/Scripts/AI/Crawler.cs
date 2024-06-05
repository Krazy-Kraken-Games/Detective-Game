using KrazyKrakenGames.DetectiveGame.Gameplay.AI;
using KrazyKrakenGames.DetectiveGame.Gameplay.Shooting;
using System;
using System.Collections;
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
        [SerializeField] private GameObject target; //This generally refers to player
        [SerializeField] private Vector3 targetDestination;

        [SerializeField] private float speed;

        [SerializeField] private EnemyState state;
        public EnemyState State => state;

        public Action<EnemyState> OnStateChangedEvent;


        [SerializeField] private LineRenderer pathRenderer;
        public int currentPathIndex = 0;

        //TO BE REMOVED:
        [SerializeField] private Transform targetLocation;


        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private int currentWaypointIndex;

        [SerializeField] private PlayerDetectionTrigger playerDetectionTrigger;

        [Header("AI Group Detection Section")]
        [SerializeField] private AIGroupDetection detectionGroup;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            agent.speed = speed;

            OnStateChangedEvent += OnLocalStateChangedHandler;

            SetState(EnemyState.IDLE);

            SetDestination(waypoints[0].position, EnemyState.PATROL);

            if(playerDetectionTrigger != null)
            {
                playerDetectionTrigger.OnPlayerDetectedEvent += OnPlayerDetectedEventHandler;
            }
        }

        private void OnDestroy()
        {
            OnStateChangedEvent -= OnLocalStateChangedHandler;

            if (playerDetectionTrigger != null)
            {
                playerDetectionTrigger.OnPlayerDetectedEvent -= OnPlayerDetectedEventHandler;
            }
        }

        private void Update()
        {
            if (state == EnemyState.PATROL)
            {
                WaypointSystem();
            }
            else if(state == EnemyState.FOLLOW)
            {
                if(target != null)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 2f)
                    {
                        //Go to attack state
                        SetState(EnemyState.ATTACK);
                        agent.destination = transform.position;

                        //Notify Ai Group detector of attack state
                        detectionGroup.AddAttacker(this);

                    }
                    else
                    {
                        SetDestination(target.transform.position, EnemyState.FOLLOW);
                    }
                }
            }
            else if(state == EnemyState.ATTACK)
            {
                if (target != null)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < 2f)
                    {
                        //Chance to attack again based on threshold
                       
                    }
                    else
                    {
                        //Switch back to follow mode
                        SetState(EnemyState.FOLLOW);
                    }
                }
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

        private void OnPlayerDetectedEventHandler(GameObject playerObject)
        {
            if(state == EnemyState.IDLE || state == EnemyState.PATROL)
            {
                Debug.Log("On player in area detected");

                SetPlayerAsTarget(playerObject);

                //Notify detection Group that I have detected the player
                detectionGroup.AddAgent(this);
            }
        }

        public void OnPlayerLeftTriggerAreaHandler()
        {
            //Notify detection Group that I detected player has left area
            detectionGroup.RemoveAgent(this);   
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

        private void SetPlayerAsTarget(GameObject _target)
        {
            if(_target != null)
            {
                target = _target;

                SetDestination(target.transform.position, EnemyState.FOLLOW);
                
            }
        }

        private void SetDestination(Vector3 _targetLocation, EnemyState _state)
        {
            targetDestination = _targetLocation;

            agent.SetDestination(targetDestination);

            SetState(_state);

        }
        #endregion


        #region Waypoint System Section

        private Vector3 NextWaypointPosition()
        {
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

            SetDestination(targetPos, EnemyState.PATROL);
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

        #region Attack Section

        [SerializeField] private bool isJumping = false;

        public void PerformAttack()
        {
            Debug.Log($"{name} is attacking!");
            StartCoroutine(JumpToTarget(target.transform.position, 1f));
        }

        /// <summary>
        /// AI jumps to the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        IEnumerator JumpToTarget(Vector3 target, float duration)
        {
            target.y += 0.8f;
            isJumping = true;
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);
                transform.LookAt(target);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = target;
            transform.LookAt(target);
            isJumping = false;
        }

        #endregion

    }
}
