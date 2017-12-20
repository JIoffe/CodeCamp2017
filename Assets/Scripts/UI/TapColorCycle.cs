using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JI.CC.Demo.UI
{
    public class TapColorCycle : MonoBehaviour, IInputClickHandler
    {
        Color[] colors;

        private int index = 0;

        public void OnInputClicked(InputClickedEventData eventData)
        {
            var i = ++index % colors.Length;
            var renderer = GetComponent<Renderer>();
            renderer.material.color = colors[i];
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