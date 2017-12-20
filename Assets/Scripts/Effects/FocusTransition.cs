using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JI.CC.Demo.Effects
{
    public class FocusTransition : MonoBehaviour, IFocusable
    {
        [Tooltip("Material to show when the element is under focus")]
        public Material activeMaterial;

        [Tooltip("Material to show when element has lost focus")]
        public Material inactiveMaterial;

        [Tooltip("The time in seconds to transition materials under focus")]
        public float transitionTime = 1f;

        private IList<Renderer> Renderers { get; set; }
        private float _fade;

        private void Awake()
        {
            Renderers = GetComponentsInChildren<Renderer>();
        }

        public void OnFocusEnter()
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo_Coroutine(1f));
        }

        public void OnFocusExit()
        {
            StopAllCoroutines();
            StartCoroutine(FadeTo_Coroutine(0));
        }

        /// <summary>
        /// Smoothly transitions to the set target to define the blend between inactive and active materials
        /// </summary>
        /// <param name="target">The target blend factor between 0 and 1</param>

        IEnumerator FadeTo_Coroutine(float target)
        {
            target = Mathf.Clamp01(target);
            var delta = target - _fade;
            var time = Mathf.Abs(delta) * transitionTime;

            var velocity = 0f;

            do
            {
                _fade = Mathf.SmoothDamp(_fade, target, ref velocity, time);
                BlendMaterials();

                yield return null;
            } while (Mathf.Abs(_fade - target) > 0.001f);

            _fade = target;
            BlendMaterials();
        }

        void BlendMaterials()
        {
            foreach(var renderer in Renderers)
            {
                renderer.material.Lerp(inactiveMaterial, activeMaterial, _fade);
            }
        }
    }
}