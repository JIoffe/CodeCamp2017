using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace JI.CC.Demo.UI
{
    public class TapSound : MonoBehaviour, IInputClickHandler
    {
        [Tooltip("The clip to play when tapped")]
        public AudioClip audioClip;

        public void OnInputClicked(InputClickedEventData eventData)
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }
}