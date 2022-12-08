//Darking Assets

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace AI
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

    public enum ItemType
    {
        HealthKit,
        AmmoBox,
    }
    public class AIItem : MonoBehaviour
    {
        [HideInInspector] public bool Health;
        [HideInInspector] public bool Ammo;
        
        [Space] 
        public AIEnums.YesNo ItemCanBeFound = AIEnums.YesNo.Yes;
        
        [Space]
        public ItemType ItemType = ItemType.HealthKit;
        [Space]

        [Condition("Health", true, 0f)]
        [Range(1f, 999f)]
        public float HealthRefillAmount = 50f;
        [Space]
        
        [Condition("Ammo", true, 0f)]
        [Range(1, 100)]
        public float AmmoRefillAmount = 30f;
        [Space]
        
        [Condition("Ammo", true, 0f)]
        public AIShooterWeapon WeaponScript = null;
        [Space]
        
        public bool PlayAnimationOnArrival;
        
        [Condition("PlayAnimationOnArrival", true, 6f)]
        public string AnimationStateName = "Pick Up";

        [Space] 
        [Header("EVENTS")] 
        [Space]
        
        public UnityEvent OnAnimationStart = new UnityEvent();
        [Space]
        
        public UnityEvent OnPickedUp = new UnityEvent();
      
        private void OnValidate()
        {
            if(Application.isPlaying)
                return;

            if (GetComponent<SphereCollider>() == null)
            {
                gameObject.AddComponent<SphereCollider>().isTrigger = true;
            }
            
            if (ItemType == ItemType.AmmoBox)
            {
                Health = false;
                Ammo = true;
            }
            else if (ItemType == ItemType.HealthKit)
            {
                Health = true;
                Ammo = false;
            }
        }

        private AISystem _system;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<AINeedsSystem>() != null ||
                other.transform.root.GetComponent<AINeedsSystem>() != null)
            {
                AISystem system = other.GetComponent<AISystem>();

                if (system == null)
                {
                    system = other.transform.root.GetComponent<AISystem>();
                }
                
                if(system == null)
                    return;

                _system = system;

                if (!PlayAnimationOnArrival)
                {
                    Pickup();
                }
                else
                {
                    _system.Anim.Play(AnimationStateName);
                }
            }
        }
        
        public void Pickup()
        {
            if (ItemType == ItemType.HealthKit)
            {
                _system.AICommandManager.SetAIHealth(_system.AICommandManager.GetHealth() + HealthRefillAmount);
                _system.GetComponent<AINeedsSystem>().Searching = false;
                Destroy(gameObject);       
            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(AIItem))]
    public class AIItemEditor : Editor
    {
        private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            style.fontSize = 13;
            EditorGUILayout.LabelField(new GUIContent(Resources.Load("LogoBrain") as Texture), style, GUILayout.ExpandWidth(true), GUILayout.Height(43));
            EditorGUILayout.LabelField("Universal AI Item", style, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    #endif
}