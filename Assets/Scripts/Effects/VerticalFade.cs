using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JI.CC.Demo.Effects
{
    public class VerticalFade : MonoBehaviour, IInputClickHandler
    {

        [Tooltip("The maximum top range of this effect")]
        public float clipRangeTop = 1f;

        [Tooltip("The minimum bottom range of this effect")]
        public float clipRangeBottom = -1f;

        [Tooltip("The time in seconds that a complete fade in or out will take")]
        public float fadeTime = 0.3f;

        [Tooltip("If true, then this vertical fade will initialize as if it has been faded out")]
        public bool hiddenByDefault = true;

        private IList<Material> Materials { get; set; }
        private float _activeClipTop;
        private float _activeClipBottom;
        private float _fadeRange;
        private float _fadeSpeed;

        private bool lastFadedOut = false;

        private void Awake()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            Materials = renderers.Select(renderer => renderer.material).ToArray();

            UpdateRange(clipRangeTop, clipRangeBottom);
        }

        public void UpdateRange(float top, float bottom)
        {
            clipRangeTop = top;
            clipRangeBottom = bottom;

            _activeClipTop = clipRangeTop;
            _fadeRange = (clipRangeTop - clipRangeBottom);
            _fadeSpeed = _fadeRange / fadeTime;

            if (hiddenByDefault)
            {
                _activeClipBottom = clipRangeTop;
            }
            else
            {
                _activeClipBottom = clipRangeBottom;
            }

            UpdateShader();
        }

        public void FadeOut()
        {
            StartCoroutine(FadeOut_Coroutine());
        }

        public void FadeIn()
        {
            StartCoroutine(FadeIn_Coroutine());
        }

        public void ToggleFade()
        {
            if (lastFadedOut)
                FadeIn();
            else
                FadeOut();
        }

        IEnumerator FadeIn_Coroutine()
        {
            enabled = true;
            lastFadedOut = false;
            _activeClipBottom = clipRangeTop;
            _activeClipTop = clipRangeTop;

            ToggleFade(true);
            while (_activeClipBottom > clipRangeBottom)
            {
                _activeClipBottom -= _fadeSpeed * Time.deltaTime;
                UpdateShader();
                yield return null;
            }
            ToggleFade(false);
            _activeClipBottom = clipRangeBottom;
        }

        IEnumerator FadeOut_Coroutine()
        {
            lastFadedOut = true;
            _activeClipBottom = clipRangeBottom;
            _activeClipTop = clipRangeTop;

            ToggleFade(true);
            while(_activeClipTop > clipRangeBottom)
            {
                _activeClipTop -= _fadeSpeed * Time.deltaTime;
                UpdateShader();
                yield return null;
            }
            _activeClipTop = clipRangeBottom;
            enabled = false;
        }

        void UpdateShader()
        {
            var y = transform.parent.transform.position.y;
            var clipTop = _activeClipTop + y;
            var clipBottom = _activeClipBottom + y;

            foreach(var material in Materials)
            {
                material.SetFloat("_clipRangeTop", clipTop);
                material.SetFloat("_clipRangeBottom", clipBottom);
            }
        }

        void ToggleFade(bool shouldFade)
        {
            var f = shouldFade ? 1f : 0f;

            foreach (var material in Materials)
            {
                material.SetFloat("_shouldFade", f);
            }
        }
        public void OnInputClicked(InputClickedEventData eventData)
        {
            ToggleFade();
        }
    }
}