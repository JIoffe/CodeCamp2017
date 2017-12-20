using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine;

namespace JI.CC.Demo.Effects
{
    public class VerticalFadeCarousel : MonoBehaviour, IInputClickHandler
    {
        [Tooltip("The maximum top range of this effect")]
        public float clipRangeTop = 1f;

        [Tooltip("The minimum bottom range of this effect")]
        public float clipRangeBottom = -1f;

        [Tooltip("Total transition time")]
        public float fadeTime = 0.4f;

        private IList<VerticalFade> FadeTargets { get; set; }
        private int _cycleIndex;

        private void Awake()
        {
            FadeTargets = GetComponentsInChildren<VerticalFade>();
        }
        void Start()
        {
            _cycleIndex = 0;
            SyncChildren();
            
            FadeTargets[0].FadeIn();
        }

        void SyncChildren()
        {
            foreach(var target in FadeTargets)
            {
                target.fadeTime = fadeTime;
                target.UpdateRange(clipRangeTop, clipRangeBottom);
                target.FadeOut();
            }
        }

        void Cycle()
        {
            var previous = FadeTargets[_cycleIndex++];
            if (_cycleIndex >= FadeTargets.Count)
                _cycleIndex = 0;

            var next = FadeTargets[_cycleIndex];

            previous.FadeOut();
            next.FadeIn();
        }
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Cycle();
        }
    }
}