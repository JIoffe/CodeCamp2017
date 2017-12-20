using HoloToolkit.Unity.InputModule;
using System.Collections;
using UnityEngine;

namespace JI.CC.Demo.UI.Characters
{
    public class EvilStatue : MonoBehaviour, ISpeechHandler, IFocusable
    {
        public string destructionKeyword = "foos ro da";
        public float destructionForce = 10f;
        public GameObject defaultMesh;
        public GameObject destructionMesh;

        [Tooltip("Time in seconds that the statue will react to player gaze")]
        public float wiggleTime = 1f;

        [Tooltip("The power with which this statue will wiggle")]
        public float wiggleStrength = 30f;

        [Tooltip("The speed of wiggle")]
        public float wiggleSpeed = 0.4f;

        [Tooltip("Sound to play upon destruction")]
        public AudioClip destructionSoundEffect;

        private bool isDestroyed = false;

        public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
        {
            if (!isDestroyed && eventData.RecognizedText.ToLowerInvariant().Equals(destructionKeyword))
            {
                DestroyStatue();
            }
        }

        public void OnFocusEnter()
        {
            if (isDestroyed)
                return;

            StopAllCoroutines();
            StartCoroutine(WiggleCoroutine());
        }

        public void OnFocusExit()
        {

        }

        void DestroyStatue()
        {
            //Disable collision and tap to place once destroyed
            var tapToPlace = GetComponent<TapToPlace>();
            if (tapToPlace != null)
                tapToPlace.enabled = false;

            var collider = GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;
        
            StopAllCoroutines();
            defaultMesh.SetActive(false);
            destructionMesh.gameObject.SetActive(true);

            var pos = Camera.main.transform.position;
            var rigidBodies = destructionMesh.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidBodies)
            {
                var d = (rb.transform.position - pos).normalized;
                rb.velocity = d * destructionForce;
            }

            isDestroyed = true;

            if (destructionSoundEffect != null)
                AudioSource.PlayClipAtPoint(destructionSoundEffect, transform.position);
        }

        IEnumerator WiggleCoroutine()
        {
            var elapsedWiggleTime = 0f;
            var startingRotation = transform.rotation;
            var startingRotationEuler = startingRotation.eulerAngles;
            var wiggle = Vector3.zero;
            while(elapsedWiggleTime < wiggleTime)
            {
                wiggle.y = Mathf.Sin(elapsedWiggleTime * wiggleSpeed) * wiggleStrength;
                transform.rotation = Quaternion.Euler(startingRotationEuler + wiggle);
                elapsedWiggleTime += Time.deltaTime;
                yield return null;
            }

            //Simple quick snap back to starting rotation
            elapsedWiggleTime = 0f;
            while(elapsedWiggleTime < wiggleTime)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, startingRotation, elapsedWiggleTime / wiggleTime);
                elapsedWiggleTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}