using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using UnityEngine;

namespace JI.CC.Demo.UI.Characters
{
    public class Elephant : MonoBehaviour, IFocusable, ISpeechHandler
    {
        [Tooltip("The clip to play when the elephant is rearing up on its hind legs")]
        public AudioClip roarAudioClip;
        [Tooltip("The clip to play when the elephant is startled")]
        public AudioClip scaredAudioClip;

        [Tooltip("The keyword that triggers levitation")]
        public string levitationKeyword = "wingardium leviosa";

        private Animator animator;
        private AudioSource audioSource;

        private bool canRoar = true;
        private bool IsLevitating { get; set; }

        private float MaxLevitationHeight { get; set; }

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        public void OnFocusEnter()
        {
            if (canRoar && !IsLevitating)
            {
                audioSource.PlayOneShot(roarAudioClip);
                animator.SetTrigger("Roar");
                canRoar = false;
                StartCoroutine(CooldownRoar_Coroutine());
            }
        }

        public void OnFocusExit()
        {
            animator.ResetTrigger("Roar");
        }

        public void OnLevitation()
        {
            if (IsLevitating)
                return;

            //Remove "TapToPlace" functionality
            var tapToPlace = GetComponent<DistantTapToPlace>();
            if (tapToPlace != null)
                tapToPlace.enabled = false;

            RemoveWorldAnchor();

            audioSource.PlayOneShot(scaredAudioClip);

            MaxLevitationHeight = transform.position.y + 1.5f;

            IsLevitating = true;
            StartCoroutine(Levitate());
            animator.SetBool("IsFloating", true);
        }

        IEnumerator Levitate()
        {
            var pos = transform.position;

            while (pos.y < MaxLevitationHeight)
            {
                pos.y += 0.3f * Time.deltaTime;
                transform.position = pos;
                yield return null;
            }

            pos.y = MaxLevitationHeight;
            transform.position = pos;

            AttachWorldAnchor();
        }

        IEnumerator CooldownRoar_Coroutine()
        {
            yield return new WaitForSeconds(10f);
            canRoar = true;
        }

        public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
        {
            var keyword = eventData.RecognizedText;

            if (keyword.Equals(levitationKeyword))
            {
                OnLevitation();
            }
        }

        private void AttachWorldAnchor()
        {
            if (WorldAnchorManager.Instance != null)
            {
                WorldAnchorManager.Instance.AttachAnchor(gameObject);
            }
        }

        private void RemoveWorldAnchor()
        {
            if (WorldAnchorManager.Instance != null)
            {
                WorldAnchorManager.Instance.RemoveAnchor(gameObject);
            }
        }
    }
}