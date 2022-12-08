using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AI
{

    public enum AttackState
    {
        Aggressive,
        Passive,
    }
    
    public enum CompanionBehaviour
    {
        Follow,
        Stay,
    }
    
    public enum PetBehaviour
    {
        Follow,
        Stay,
    }
public class AICommandManager : MonoBehaviour
{
    [Serializable]
    public class companionCommands
    {
        [HideInInspector] public AISystem AISystem;
        
        /// <summary>
        /// Changes The Companion AI's Attack State (Aggressive, Passive)
        /// </summary>
        public void ChangeAttackState(AttackState newAttackState)
        {
            AISystem.TypeSettings.CompanionSettings.AttackState = newAttackState;
        }
        
        /// <summary>
        /// Changes The Companion AI's Behaviour (Follow, Stay)
        /// </summary>
        public void ChangeCompanionBehaviour(CompanionBehaviour newBehaviour)
        {
            AISystem.TypeSettings.CompanionSettings.companionBehaviour = newBehaviour;
        }
        
    }
    
    [Serializable]
    public class petCommands
    {
        [HideInInspector] public AISystem AISystem;
        
        /// <summary>
        /// Changes The Pet AI's Behaviour (Follow, Stay)
        /// </summary>
        public void ChangePetBehaviour(PetBehaviour newBehaviour)
        {
            AISystem.TypeSettings.PetSettings.PetBehaviour = newBehaviour;
        }
        
    }
    
    [HideInInspector] public AISystem AISystem;
    [HideInInspector] public companionCommands CompanionCommands;
    [HideInInspector] public petCommands PetCommands;
    private void Awake()
    {
        if (AISystem == null)
        {
            AISystem = gameObject.GetComponent<AISystem>();
        }

        CompanionCommands = new companionCommands();
        CompanionCommands.AISystem = AISystem;
        
        PetCommands = new petCommands();
        PetCommands.AISystem = AISystem;

    }
    

    /// <summary>
    /// Adds A GameObject To The Ignored List For Detection
    /// </summary>
    public void AddIgnoredTarget(GameObject Target)
    {
        if (Target != null)
        {
            if(!AISystem.Detection.DetectionSettings.IgnoredObjects.Contains(Target))
                AISystem.Detection.DetectionSettings.IgnoredObjects.Add(Target);
        }
    }
    
    /// <summary>
    /// Removes The GameObject From The Ignored List For Detection
    /// </summary>
    public void RemoveIgnoredTarget(GameObject Target)
    {
        if (Target != null)
        {
            if(AISystem.Detection.DetectionSettings.IgnoredObjects.Contains(Target))
                AISystem.Detection.DetectionSettings.IgnoredObjects.Remove(Target);
        }
    }
    
    /// <summary>
    /// Removes Every GameObject From The Ignored List For Detection
    /// </summary>
    public void ClearAllIgnoredTargets()
    {
        AISystem.Detection.DetectionSettings.IgnoredObjects.Clear();
    }
    
    /// <summary>
    /// Returns The Current Target GameObject Of The AI / Will Return Null If There Is Currently No Target
    /// </summary>
    public GameObject GetTarget()
    {
        return AISystem.Target;
    }
    
    /// <summary>
    /// Sets The Temporary Target Of The AI
    /// </summary>
    public void SetTarget(GameObject newTarget)
    {
        AISystem.SearchSettedTarget = true;
        AISystem.Target = newTarget;
    }

    /// <summary>
    /// Sets The Temporary Destination For The AI (If The AI Isn't Wandering, AI Will Go To The Destination Whenever It Wanders Again)
    /// </summary>
    public void SetDestination(Vector3 newDestination)
    {
#if !AI_Integration_PathfindingPro
        AISystem.Nav.ResetPath();
#else
        AISystem.NavPro.SetPath(null);
#endif
        AISystem.OvverideWandering = true;
        AISystem.OvverideWanderingPos = newDestination;
    }
    
    /// <summary>
    /// Changes The AI's Current Detection Type
    /// </summary>
    public void ChangeDetectionType(AIEnums.DetectionType detectionType)
    {
        AISystem.Detection.DetectionSettings.DetectionType = detectionType;
    }
   
    /// <summary>
    /// Changes The AI's Current Faction
    /// </summary>
    public void ChangeFaction(AIEnums.Factions factions)
    {
        AISystem.Detection.Factions.Factions = factions;
    }
    
    /// <summary>
    /// Returns The AI's Current Detection Type
    /// </summary>
    public AIEnums.DetectionType GetDetectionType()
    {
        return AISystem.Detection.DetectionSettings.DetectionType;
    }

    /// <summary>
    /// Returns The AI's Current Faction
    /// </summary>
    public AIEnums.Factions GetFaction()
    {
        return AISystem.Detection.Factions.Factions;
    }
    
    /// <summary>
    /// Kills The AI With A Delay Or Instantly
    /// </summary>
    public void KillAI(float delay = 0f)
    {
       Invoke("Kill", delay);
    }

    private void Kill()
    {
        AISystem.Die();
    }
    
    /// <summary>
    /// Freezes The AI Delayed Or Instantly
    /// </summary>
    public void FreezeAI(float delay = 0f)
    {
        Invoke("Freeze", delay);
    }

    private void Freeze()
    {
        AISystem.FreezeAIManuel(true);
    }

    /// <summary>
    /// Stops The AI Scripts, So User Can Add Custom Behaviours
    /// </summary>
    public void StopAIBehaviour()
    {
        AISystem.StopAIBehaviour = true;
        AISystem.AdjustNavmesh(true);
        AISystem.PlayAnimation(AIEnums.AnimationType.Idle);
    }
    
    /// <summary>
    /// Starts The AI Scripts Again
    /// </summary>
    public void StartAIBehaviour()
    {
        AISystem.StopAIBehaviour = false;
    }
    
    /// <summary>
    /// Unfreezes The AI Delayed Or Instantly
    /// </summary>
    public void UnFreezeAI(float delay = 0f)
    {
        Invoke("UnFreeze", delay);
    }

    private void UnFreeze()
    {
        AISystem.FreezeAIManuel(false);
    }

    /// <summary>
    /// Revives The AI
    /// </summary>
    public void ReviveAI()
    {
        AISystem.Revive();
    }
    
    /// <summary>
    /// Sets The AI Health
    /// </summary>
    public void SetAIHealth(float newHealth)
    {
        AISystem.Health = newHealth;
    }
    
    /// <summary>
    /// Refills The AI Health Instantly
    /// </summary>
    public void RefillAIHealth()
    {
        AISystem.Health = AISystem.Stats.MaxHealth;
    }
    
    /// <summary>
    /// Returns The Current AI Health
    /// </summary>
    public float GetHealth()
    {
       return AISystem.Health;
    }
    
    /// <summary>
    /// Changes The AI Type
    /// </summary>
    public void ChangeAIType(AIEnums.AIType AIType)
    {
        AISystem.General.AIType = AIType;
    }
    
    /// <summary>
    /// Changes The AI Confidence
    /// </summary>
    public void ChangeAIConfidence(AIEnums.AIConfidence AIConfidence)
    {
        AISystem.General.AIConfidence = AIConfidence;
    }
    
    /// <summary>
    /// Changes The AI's Wandering Type
    /// </summary>
    public void ChangeAIWanderType(AIEnums.WanderType wanderType)
    { 
        AISystem.General.wanderType = wanderType;
    }
    
    /// <summary>
    /// Returns The Current AI Type
    /// </summary>
    public AIEnums.AIType GetAIType()
    {
        return AISystem.General.AIType;
    }
    
    /// <summary>
    /// Returns The Current AI Confidence
    /// </summary>
    public AIEnums.AIConfidence GetAIConfidence()
    {
       return AISystem.General.AIConfidence;
    }
    
    /// <summary>
    /// Returns The Current AI Wandering Type
    /// </summary>
    public AIEnums.WanderType GetAIWanderType()
    {
        return AISystem.General.wanderType;
    }

    /// <summary>
    /// Returns The Current Distance Between AI And AI'S Current Target. (Returns -1 If The Target Is Null)
    /// </summary>
    public float GetTargetDistance()
    {
        if (GetTarget() == null)
        {
            return -1f;
        }
        else
        {
            return AISystem.TargetDistance;
        }
    }

    /// <summary>
    /// Enables The Health Regeneration Of The AI
    /// </summary>
    public void EnableHealthRegeneration()
    {
        AISystem.Stats.AIRegeneratesHealth = AIEnums.YesNo.Yes;
    }
    
    /// <summary>
    /// Disables The Health Regeneration Of The AI
    /// </summary>
    public void DisableHealthRegeneration()
    {
        AISystem.Stats.AIRegeneratesHealth = AIEnums.YesNo.No;
    }
}
#if UNITY_EDITOR
    
[CustomEditor(typeof(AICommandManager))]
public class AICommandManagerEditor : Editor
{
    private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};
        
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, _dontIncludeMe);
            
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
    
}