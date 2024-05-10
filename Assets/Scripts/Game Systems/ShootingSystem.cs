using Cinemachine;
using KrazyKrakenGames.DetectiveGame.Gameplay.Shooting;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Gameplay.Feature.Shooting
{
    public class ShootingSystem : MonoBehaviour
    {
        [Header("Projectile Spawn System")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        private Vector3 raycastHitPosition = Vector3.zero;

        //Used for IK
        [SerializeField] private Transform aimTarget;

        //Gun references
        [Space(5)]
        [Header("Weapon References")]
        [SerializeField] private GameObject pistol;

        private void Update()
        {
            SetAimTargetPosition();
        }

        public void ResetRaycastHit()
        {
            raycastHitPosition = Vector3.zero;
        }

        

        public void PreShootRaycasting()
        {
            Camera cam = Camera.main.GetComponent<CinemachineBrain>().OutputCamera;

            Vector3 originalPos = UIManager.instance.CrossHairWorldPosition;

            //NOTE: Final destination might need tweaking in logic after IK animations are up
            Vector3 finalDestination = originalPos + (cam.transform.forward * 10f);

            Vector3 direction = finalDestination - (projectileSpawnPoint.position);
            RaycastHit hit;

            var ray = new Ray(projectileSpawnPoint.position, direction);
            if (Physics.Raycast(projectileSpawnPoint.position, direction, out hit, Mathf.Infinity))
            {
                if (hit.collider != null)
                {
                    Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0.1f);
                    raycastHitPosition = hit.point;
                }
                else
                {
                    raycastHitPosition = finalDestination;
                }
            }
            else
            {
                raycastHitPosition = finalDestination;
            }
        }

        public void Shoot()
        {
            //TODO: Should force be used instead of direction and velocity?
            Vector3 direction = raycastHitPosition - (projectileSpawnPoint.position);

            direction.Normalize();

            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.SetDirection(direction);

        }

        private void SetAimTargetPosition()
        {
            //Aim target position should have similar math to Shoot logic
            if (raycastHitPosition != Vector3.zero)
            {
                aimTarget.position = raycastHitPosition;
            }
            else
            {
                //Turn weight offline
            }
        }

        public void ActivateShootMode()
        {
            ResetRaycastHit();
            pistol.SetActive(true);
        }

        public void DeactivateShootMode()
        {
            ResetRaycastHit();
            pistol.SetActive(false);
        }
    }
}
