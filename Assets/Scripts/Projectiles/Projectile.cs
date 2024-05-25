using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Global;
using System.IO.Pipes;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Shooting
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        [SerializeField] private float projectileSpeed;

        [SerializeField] private Vector3 fireDirection;

        private bool fire = false;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            projectileSpeed = GamePlayConstants.ProjectileSpeed;
        }

        private void FixedUpdate()
        {
            if (!fire) return;

            rb.velocity = fireDirection * projectileSpeed;
        }

        #region MOVEMENT

        public void SetDirection(Vector3 direction)
        {
            // Normalize the direction to ensure consistent speed regardless of its magnitude
            direction.Normalize();
            // Set the rotation of the projectile to face the specified direction
            //transform.rotation = Quaternion.LookRotation(direction);
            fireDirection = direction;

            fire = true;
        }

        #endregion

        #region COLLISION

        private void OnCollisionEnter(Collision collision)
        {
            var collidedWith = collision.gameObject;
            if (collidedWith.tag == "Target")
            {
                Destroy(collidedWith);
            }
            else if(collidedWith.tag == "Enemy")
            {
                if (collidedWith.TryGetComponent<Crawler>(out var crawler))
                {
                    crawler.OnCollisionWithProjectile(this);
                }
            }

            Destroy(gameObject);
        }

        #endregion


    }
}
