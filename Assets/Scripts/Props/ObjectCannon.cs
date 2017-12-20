using System.Collections;
using UnityEngine;

namespace JI.CC.Demo.UI.Props
{
    public class ObjectCannon : MonoBehaviour
    {
        [Tooltip("Prefab to use as projectile")]
        public GameObject projectile;

        [Tooltip("The angle that projectiles will spread in")]
        public float spreadAngle = 60f;

        [Tooltip("The force with which projectiles will be launched")]
        public float launchForce = 2f;

        [Tooltip("The number of projectiles to launch")]
        public float numberOfProjectiles = 12f;

        public void Fire()
        {
            StartCoroutine(Fire_Coroutine());
        }

        private IEnumerator Fire_Coroutine()
        {
            for (var i = 0; i < numberOfProjectiles; ++i)
            {
                var instance = Instantiate(projectile);

                instance.transform.position = transform.position;
                instance.transform.rotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));

                var rb = instance.GetComponentInChildren<Rigidbody>();

                if (rb != null)
                {
                    var q = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0f);
                    var force = q * transform.forward * launchForce;

                    rb.AddForce(force);
                }


                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
    }
}