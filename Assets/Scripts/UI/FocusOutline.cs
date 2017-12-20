using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JI.CC.Demo.UI
{
    /// <summary>
    /// Copies all mesh renderers of an object to create outline meshes that appear on focus
    /// </summary>
    public class FocusOutline : MonoBehaviour, IFocusable
    {
        [Tooltip("The material to use for the on focus effect")]
        public Material outlineMaterial;

        private IList<GameObject> OutlineObjects { get; set; }

        private void Awake()
        {
            CreateOutlineObjects();
        }

        public void OnFocusEnter()
        {
            foreach (var outline in OutlineObjects)
                outline.SetActive(true);
        }

        public void OnFocusExit()
        {
            foreach (var outline in OutlineObjects)
                outline.SetActive(false);
        }

        private void CreateOutlineObjects()
        {
            var meshes = GetComponentsInChildren<MeshFilter>();
            OutlineObjects = meshes.Select(mesh =>
            {
                var outlineObject = new GameObject("Outline");
                outlineObject.AddComponent<MeshFilter>();
                outlineObject.AddComponent<MeshRenderer>();

                outlineObject.GetComponent<MeshFilter>().sharedMesh = mesh.sharedMesh;

                outlineObject.transform.parent = transform;
                outlineObject.transform.position = transform.position;
                outlineObject.transform.rotation = transform.rotation;
                outlineObject.transform.localScale = Vector3.one;

                var outlineRenderer = outlineObject.GetComponent<Renderer>();
                outlineRenderer.material = outlineMaterial;

                outlineObject.SetActive(false);

                return outlineObject;
            })
            .ToArray();
        }
    }
}