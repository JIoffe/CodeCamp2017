using HoloToolkit.Unity.InputModule;
using JI.CC.Demo.Voice;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace JI.CC.Demo.UI.Props
{
    public class Journal : MonoBehaviour, ISpeechHandler, IDictationListener
    {
        [Tooltip("Keyword that starts the journal dictation")]
        public string startDictationKeyword = "dear journal";

        [Tooltip("Keyword that ends the journal dictation")]
        public string endDictationKeyword = "the end";

        [Tooltip("Maximum length of a line in characters")]
        public int maxLineLength = 11;

        private TextMesh TextMesh { get; set; }

        private readonly StringBuilder _DictationBuffer = new StringBuilder();
        private StringBuilder DictationBuffer
        {
            get
            {
                return _DictationBuffer;
            }
        }

        private readonly StringBuilder _LineBuffer = new StringBuilder();
        private StringBuilder LineBuffer
        {
            get
            {
                return _LineBuffer;
            }
        }

        private readonly StringBuilder _WritingBuffer = new StringBuilder();
        private StringBuilder WritingBuffer
        {
            get
            {
                return _WritingBuffer;
            }
        }

        public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
        {
            var keyword = eventData.RecognizedText.ToLowerInvariant();
            if (keyword.Equals(startDictationKeyword))
            {
                BeginDictation();
            }
        }

        private void Awake()
        {
            TextMesh = GetComponentInChildren<TextMesh>();
        }
        void BeginDictation()
        {
            DictationManager.Instance.RequestDictation(this);
            DictationBuffer.Length = 0;
        }

        void SetText(string text)
        {
            WritingBuffer.Length = 0;
            LineBuffer.Length = 0;

            WritingBuffer.AppendLine("Dear Journal, ");

            var words = text.Split(null);

            foreach(var word in words)
            {
                if (LineBuffer.Length > 0)
                    LineBuffer.Append(' ');

                LineBuffer.Append(word);
                if(LineBuffer.Length >= maxLineLength)
                {
                    WritingBuffer.AppendLine(LineBuffer.ToString());
                    LineBuffer.Length = 0;
                }
            }

            TextMesh.text = WritingBuffer.ToString();
        }

        public void OnDictationHypothesis(string text)
        {
            SetText(DictationBuffer.ToString() + ' ' + text + "...");
        }

        public void OnDictationResult(string text, ConfidenceLevel confidence)
        {
            if (text.ToLowerInvariant().Contains(endDictationKeyword))
            {
                DictationManager.Instance.EndDictation();
                return;
            }

            DictationBuffer.Append(text).Append('.');
            SetText(DictationBuffer.ToString());
        }

        public void OnDictationComplete(DictationCompletionCause cause)
        {

        }

        public void OnDictationError(string error, int result)
        {
            TextMesh.text = "[ Dictation Error :( ]";
        }
    }
}