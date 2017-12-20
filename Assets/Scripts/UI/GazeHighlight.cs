using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JI.CC.Demo.UI
{
    [RequireComponent(typeof(Renderer))]
    public class GazeHighlight : MonoBehaviour, IFocusable
    {
        public Color unfocusedColor;
        public Color focusedColor;

        public void OnFocusEnter()
        {
            var renderer = GetComponent<Renderer>();
            renderer.material.color = focusedColor;
        }

        public void OnFocusExit()
        {
            var renderer = GetComponent<Renderer>();
            renderer.material.color = unfocusedColor;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}