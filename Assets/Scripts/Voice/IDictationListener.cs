using UnityEngine.Windows.Speech;

namespace JI.CC.Demo.Voice
{
    public interface IDictationListener
    {
        void OnDictationHypothesis(string text);
        void OnDictationResult(string text, ConfidenceLevel confidence);
        void OnDictationComplete(DictationCompletionCause cause);
        void OnDictationError(string error, int result);
    }
}