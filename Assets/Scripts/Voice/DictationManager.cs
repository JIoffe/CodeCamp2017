using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace JI.CC.Demo.Voice
{
    public class DictationManager : Singleton<DictationManager>
    {
        [Tooltip("The time in seconds that the dictation engine will wait for the user to begin speaking before timing out.")]
        public float initialDictationSilenceTimeout = 5.0f;

        [Tooltip("The timeout in seconds before ending the dictation stream.")]
        public float dictationSilenceTimeout = 15.0f;

        private DictationRecognizer DictationRecognizer { get; set; }

        private IDictationListener ActiveListener { get; set; }

        public void RequestDictation(IDictationListener listener)
        {
            ClearListener();

            ActiveListener = listener;
            DictationRecognizer.DictationHypothesis += ActiveListener.OnDictationHypothesis;
            DictationRecognizer.DictationResult += ActiveListener.OnDictationResult;
            DictationRecognizer.DictationComplete += ActiveListener.OnDictationComplete;
            DictationRecognizer.DictationError += ActiveListener.OnDictationError;

            StartDictation();
        }

        private void StartDictation()
        {
            ShutdownPhraseRecognitionSystem();

            if (DictationRecognizer.Status != SpeechSystemStatus.Running)
                DictationRecognizer.Start();
        }
        public void EndDictation()
        {
            ClearListener();

            if (DictationRecognizer.Status == SpeechSystemStatus.Running)
                DictationRecognizer.Stop();

            RestartPhraseRecognitionSystem();
        }

        private void ClearListener()
        {
            if (ActiveListener == null)
                return;

            DictationRecognizer.DictationHypothesis -= ActiveListener.OnDictationHypothesis;
            DictationRecognizer.DictationResult -= ActiveListener.OnDictationResult;
            DictationRecognizer.DictationComplete -= ActiveListener.OnDictationComplete;
            DictationRecognizer.DictationError -= ActiveListener.OnDictationError;

            ActiveListener = null;
        }

        void Start()
        {
            DictationRecognizer = new DictationRecognizer();
            DictationRecognizer.InitialSilenceTimeoutSeconds = initialDictationSilenceTimeout;
            DictationRecognizer.AutoSilenceTimeoutSeconds = dictationSilenceTimeout;

            DictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
            DictationRecognizer.DictationError += DictationRecognizer_DictationError;
        }

        override protected void OnDestroy()
        {
            DictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
            DictationRecognizer.DictationError -= DictationRecognizer_DictationError;

            DictationRecognizer = null;
        }

        private void ShutdownPhraseRecognitionSystem()
        {
            if (PhraseRecognitionSystem.Status != SpeechSystemStatus.Running)
                return;

            PhraseRecognitionSystem.Shutdown();
        }

        private void RestartPhraseRecognitionSystem()
        {
            if (PhraseRecognitionSystem.Status == SpeechSystemStatus.Running)
                return;

            PhraseRecognitionSystem.Restart();
        }

        private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
        {
            Debug.LogWarning("Dictation Complete: " + cause);
            EndDictation();
        }

        private void DictationRecognizer_DictationError(string error, int result)
        {
            Debug.LogError("Dictation Error: " + error);
            EndDictation();
        }
    }
}