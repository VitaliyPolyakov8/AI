using System;
using System.Collections.Generic;

using UnityEngine;

namespace AI
{
    public class AIManager : MonoBehaviour
    {
        public static List<AISystem> AISystems = new List<AISystem>();

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            AIManager[] managers = FindObjectsOfType<AIManager>();

            foreach (var manager in managers)
            {
                if (manager != this)
                    DestroyImmediate(this);
            }
        }

        public static void SoundDetection(AIEnums.SoundType soundType, float soundRadius, Transform soundSource)
        {
            foreach (var AI in AISystems)
            {
                if(AI != null && AI.gameObject.activeInHierarchy && AI.transform != soundSource)
                    AI.CheckSound(soundRadius, soundSource);
            }
        }
    }
}