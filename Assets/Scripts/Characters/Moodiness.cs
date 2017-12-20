using System;
using System.Collections.Generic;
using UnityEngine;

namespace JI.CC.Demo.UI.Characters
{
    public class Moodiness : MonoBehaviour
    {
        [Tooltip("Speed per seconds that mood will change")]
        public float moodReactionSpeed = 1f;

        private IDictionary<Mood, IList<KeyValuePair<SkinnedMeshRenderer, int>>> _MoodTargetDictionary = new Dictionary<Mood, IList<KeyValuePair<SkinnedMeshRenderer, int>>>();
        private IDictionary<Mood, IList<KeyValuePair<SkinnedMeshRenderer, int>>> MoodTargetDictionary { get { return _MoodTargetDictionary; } }

        private float[] moodWeights;
        private float[] moodRenderWeights;

        // Use this for initialization
        void Start()
        {
            moodWeights = new float[Enum.GetValues(typeof(Mood)).Length];
            moodRenderWeights = new float[moodWeights.Length];
            BindMoodTargets();
        }

        private void Update()
        {
            for (var i = 0; i < moodWeights.Length; ++i)
            {
                moodRenderWeights[i] = Mathf.Lerp(moodRenderWeights[i], moodWeights[i], moodReactionSpeed * Time.deltaTime);
            }

            RenderCurrentMood();
        }
        
        public void ChangeMood(Mood mood)
        {
            var iMood = (int)mood;
            for (var i = 0; i < moodWeights.Length; ++i)
                moodWeights[i] = (i == iMood) ? 100f : 0f;
        }

        public void ClearMood()
        {
            ChangeMood(Mood.Neutral);
        }

        private void RenderCurrentMood()
        {
            for (var i = 0; i < moodRenderWeights.Length; ++i)
            {
                var matchingRenderers = MoodTargetDictionary[(Mood)i];
                foreach (var renderer in matchingRenderers)
                {
                    var mesh = renderer.Key;
                    var index = renderer.Value;

                    mesh.SetBlendShapeWeight(index, moodRenderWeights[i]);
                }
            }
        }

        void BindMoodTargets()
        {
            var skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            //Go for each mood and extract matching renderers and blend targets
            foreach (var mood in Enum.GetValues(typeof(Mood)))
            {
                var moodName = mood.ToString().ToLowerInvariant();
                var matchingEntries = new List<KeyValuePair<SkinnedMeshRenderer, int>>();

                foreach(var skinnedMeshRenderer in skinnedMeshRenderers)
                {
                    var sharedMesh = skinnedMeshRenderer.sharedMesh;

                    for(var i = 0; i < sharedMesh.blendShapeCount; ++i)
                    {
                        var blendShapeName = sharedMesh.GetBlendShapeName(i);
                        if (blendShapeName.ToLowerInvariant().Contains(moodName))
                        {
                            matchingEntries.Add(new KeyValuePair<SkinnedMeshRenderer, int>(skinnedMeshRenderer, i));
                            break;
                        }
                    }
                }

                MoodTargetDictionary[(Mood)mood] = matchingEntries;
            }
        }
    }
}