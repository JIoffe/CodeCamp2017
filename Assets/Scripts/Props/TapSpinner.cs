using HoloToolkit.Unity.InputModule;
using UnityEngine;
using JI.CC.Demo.Effects;

namespace JI.CC.Demo.UI.Props
{
    public class TapSpinner : MonoBehaviour, IInputClickHandler
    {
        [Tooltip("The prefab to use for a particle effect when tapped")]
        public OneShotParticle tapParticleEffect;

        [Tooltip("The velocity added to the spin when tapped")]
        public float spinStrength = 4f;

        [Tooltip("The amount of velocity lost per second")]
        public float rotationalDrag = 0.02f;

        private Vector3 rotationalVelocity = Vector3.zero;

        public void OnInputClicked(InputClickedEventData eventData)
        {
            FireParticles();

            var weights = new Vector3
            {
                x = Random.Range(-1f, 1f),
                y = Random.Range(-1f, 1f),
                z = Random.Range(-1f, 1f)
            };

            weights.Normalize();
            weights.x *= spinStrength;
            weights.y *= spinStrength;
            weights.z *= spinStrength;

            rotationalVelocity = weights;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotationalVelocity * Time.deltaTime);
            rotationalVelocity = Vector3.Lerp(rotationalVelocity, Vector3.zero, rotationalDrag * Time.deltaTime);
        }

        void FireParticles()
        {
            if (tapParticleEffect == null)
                return;

            var effect = Instantiate(tapParticleEffect);
            effect.transform.position = transform.position;
        }
    }
}