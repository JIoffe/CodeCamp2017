using UnityEngine;

namespace JI.CC.Demo.Effects
{
    [RequireComponent(typeof(ParticleSystem))]
    public class OneShotParticle : MonoBehaviour
    {
        private ParticleSystem ParticleSystem { get; set; }
        // Use this for initialization
        void Start()
        {
            ParticleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.Play();
        }

        // Update is called once per frame
        void Update()
        {
            if (!ParticleSystem.isPlaying)
                Destroy(gameObject);
        }
    }
}