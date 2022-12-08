using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{

public class AIIntegrationsManager : MonoBehaviour
{
    private void Start()
    {
        AddIntegration();
    }

    private void AddIntegration()
    {
        
#if AI_Integration_INVECTOR && (INVECTOR_SHOOTER || INVECTOR_MELEE)

        if(gameObject.GetComponent<AIInvectorIntegration>() == null)
            gameObject.AddComponent<AIInvectorIntegration>();

        AIDamageableObject[] damageableObjects = GetComponentsInChildren<AIDamageableObject>();
      
        
        foreach (var damageableObject in damageableObjects)
        {
            if(damageableObject.gameObject.GetComponent<AIInvectorIntegration>() == null)
                damageableObject.gameObject.AddComponent<AIInvectorIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_HQFPS && HQ_FPS_TEMPLATE

        if(gameObject.GetComponent<AIHqfpsIntegration>() == null)
            gameObject.AddComponent<AIHqfpsIntegration>();

        AIDamageableObject[] damageableObjectsHQFPS = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsHQFPS)
        {
            if(damageableObject.gameObject.GetComponent<AIHqfpsIntegration>() == null)
                damageableObject.gameObject.AddComponent<AIHqfpsIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_STP && SURVIVAL_TEMPLATE_PRO

        if(gameObject.GetComponent<AISTPIntegration>() == null)
            gameObject.AddComponent<AISTPIntegration>();

        AIDamageableObject[] damageableObjectsSTP = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsSTP)
        {
            if(damageableObject.gameObject.GetComponent<AISTPIntegration>() == null)
                damageableObject.gameObject.AddComponent<AISTPIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_GKC

        if(gameObject.GetComponent<AIGKCIntegration>() == null)
            gameObject.AddComponent<AIGKCIntegration>();

        AIDamageableObject[] damageableObjectsGKC = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsGKC)
        {
            if(damageableObject.gameObject.GetComponent<AIGKCIntegration>() == null)
                damageableObject.gameObject.AddComponent<AIGKCIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_MMFPSE && INTEGRATION_FPV2NEWER

        if(gameObject.GetComponent<AIMmfpseIntegration>() == null)
            gameObject.AddComponent<AIMmfpseIntegration>();

        AIDamageableObject[] damageableObjectsMMFPSE = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsMMFPSE)
        {
            if(damageableObject.gameObject.GetComponent<AIMmfpseIntegration>() == null)
                damageableObject.gameObject.AddComponent<AIMmfpseIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_NEOFPS && NEOFPS

        if(gameObject.GetComponent<AINeofpsIntegration>() == null)
            gameObject.AddComponent<AINeofpsIntegration>();

        AIDamageableObject[] damageableObjectsNEOFPS = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsNEOFPS)
        {
            if(damageableObject.gameObject.GetComponent<AINeofpsIntegration>() == null)
                damageableObject.gameObject.AddComponent<AINeofpsIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_OPSIVE && (THIRD_PERSON_CONTROLLER || FIRST_PERSON_CONTROLLER)

        if(gameObject.GetComponent<AIOpsiveIntegration>() == null)
            gameObject.AddComponent<AIOpsiveIntegration>();

        AIDamageableObject[] damageableObjectsOPSIVE = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsOPSIVE)
        {
            if(damageableObject.gameObject.GetComponent<AIOpsiveIntegration>() == null)
                damageableObject.gameObject.AddComponent<AIOpsiveIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_HORRORFPS

        if(gameObject.GetComponent<AIHorrorfpsIntegration>() == null)
            gameObject.AddComponent<AIHorrorfpsIntegration>();

        AIDamageableObject[] damageableObjectsHORRORFPS = GetComponentsInChildren<AIDamageableObject>();
      
        foreach (var damageableObject in damageableObjectsHORRORFPS)
        {
            if(damageableObject.gameObject.GetComponent<AIHorrorfpsIntegration>() == null)
                damageableObject.gameObject.AddComponent<AIHorrorfpsIntegration>();
        }
        
        Destroy(this);
#endif
        
#if AI_Integration_USK

        //Do This After 1.7 Update USK
#endif
    }
}
    
}