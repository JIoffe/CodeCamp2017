using HoloToolkit.Unity.InputModule;
using UnityEngine;
using System;
using JI.CC.Demo.UI.Props;

namespace JI.CC.Demo.UI.Characters
{
    public class FriendlyRobot : MonoBehaviour, ISpeechHandler
    {
        [Serializable]
        public class Barks
        {
            [Tooltip("Sound to use as a generic greeting")]
            public AudioClip greeting;

            [Tooltip("Sound to use when character is annoyed")]
            public AudioClip annoyed;

            [Tooltip("Sound to use when performing an action")]
            public AudioClip action;
        }

        [Tooltip("The keyword to respond to as a greeting")]
        public string greetingKeyword = "hello";

        [Tooltip("the keyword to respond to as a scolding")]
        public string scoldKeyword = "bad robot";

        [Tooltip("The keyword to invoke this robot's primary action")]
        public string actionKeyword = "make it rain";

        [Tooltip("Barks to respond to the user with")]
        public Barks barks;

        private Animator Animator { get; set; }
        private Moodiness Moodiness { get; set; }
        private ObjectCannon ObjectCannon { get; set; }


        public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
        {
            var keyword = eventData.RecognizedText;
            if(keyword.Equals(greetingKeyword))
            {
                OnGreeting();
            }else if(keyword.Equals(scoldKeyword)){
                OnScold();
            }else if(keyword.Equals(actionKeyword))
            {
                OnAction();
            }
        }

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            Moodiness = GetComponent<Moodiness>();
            ObjectCannon = GetComponentInChildren<ObjectCannon>();
        }


        void OnGreeting()
        {
            TriggerAnimation("Greeting");
            PlayClip(barks.greeting);
            SetMood(Mood.Excited);
        }

        void OnScold()
        {
            TriggerAnimation("Sulk");
            PlayClip(barks.annoyed);
            SetMood(Mood.Sad);
        }

        void OnAction()
        {
            TriggerAnimation("Emphasize");
            PlayClip(barks.action);
            SetMood(Mood.Happy);
            ObjectCannon.Fire();
        }

        void TriggerAnimation(string name)
        {
            if (Animator != null)
                Animator.SetTrigger(name);
        }
        void SetMood(Mood mood)
        {
            if (Moodiness != null)
                Moodiness.ChangeMood(mood);
        }

        void PlayClip(AudioClip clip)
        {
            if (clip != null)
                AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}