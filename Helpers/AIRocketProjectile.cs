using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AI
{


    [RequireComponent(typeof(SphereCollider))]
public class AIRocketProjectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [HideInInspector] public AIShooterWeapon weapon;
    [HideInInspector] public GameObject damager;
    
    [Space] 
    [Header("SETTINGS")] 
    [Space] 
    
    public float ExplosionRadius = 6f;
    [Space] [Space] [Help("How Far Should The Explosion Affect?", HelpBoxMessageType.Info)]

    public AIEnums.YesNo ImpulseOnImpact = AIEnums.YesNo.Yes;
    [Space] [Space] [Help("Should The Explosion Affect Near Rigidbodies?", HelpBoxMessageType.Info)]
    
    public float ImpulseForce = 20f;
    [Space] [Space] [Help("Impulse Force That Will Be Applied On Near Rigidbodies.", HelpBoxMessageType.Info)]
    
    public GameObject ExplosionParticle = null;
    [Space] [Space] [Help("The Explosion Particle Object.", HelpBoxMessageType.Info)]
    
    [Space]
    [Header("EVENTS")]
    [Space]

    public UnityEvent OnExplode = new UnityEvent();
    [Space]
    
    public UnityEvent OnDamaged = new UnityEvent();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }

    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private List<AIDamageable> Damageables = new List<AIDamageable>();
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == gameObject ||
            other.transform.parent != null && other.transform.parent.gameObject == gameObject)
        {
            return;
        }
        
        Collider[] Affect = Physics.OverlapSphere(transform.position, ExplosionRadius);

        Damageables.Clear();
        foreach (var Obj in Affect)
        {
            
            if (Obj.GetComponent<AIDamageable>() != null ||
                Obj.GetComponent<AIPlayerReference>() != null)
            {
                AIDamageable AIDamageable = Obj.GetComponent<AIDamageable>();

                if(Damageables.Contains(AIDamageable))
                    continue;
                
                Damageables.Add(AIDamageable);
                
                if (AIDamageable != null)
                {
                    // if (AIDamageable is AISystem)
                    // {
                    //     if(AIDamageable.Equals(weapon.AISystem))
                    //         return;
                    // }
                    if (AIDamageable is AIDamageableObject)
                    {
                        AIDamageableObject AIDamageables = AIDamageable as AIDamageableObject;
                        if(AIDamageables.AISystem.Equals(weapon.AISystem))
                            return;
                    }
                    
                    float DamageValue = damage;
                    AIDamageable.TakeDamage(DamageValue, AttackerType.AI, damager);
                    OnDamaged.Invoke();
                    
                }
                else
                {
                    AIPlayerReference PlayerReference = Obj.GetComponent<AIPlayerReference>();
                    
                    if (PlayerReference != null)
                    {
                        float DamageValue = damage;
                        PlayerReference.TakeDamage(DamageValue, AttackerType.AI, damager);
                        OnDamaged.Invoke();
                        
                    }
                }
            }
            else if (Obj.transform.root.GetComponent<AIDamageable>() != null ||
                     Obj.transform.root.GetComponent<AIPlayerReference>() != null)
            {
                GameObject RootObj = Obj.transform.root.gameObject;
                
                AIDamageable AIDamageable = RootObj.GetComponent<AIDamageable>();

                if(Damageables.Contains(AIDamageable))
                    continue;
                
                Damageables.Add(AIDamageable);
                
                if (AIDamageable != null)
                {
                    if (AIDamageable is AISystem)
                    {
                        if(AIDamageable.Equals(weapon.AISystem))
                            return;
                    }
                    if (AIDamageable is AIDamageableObject)
                    {
                        AIDamageableObject AIDamageables = AIDamageable as AIDamageableObject;
                        if(AIDamageables.AISystem.Equals(weapon.AISystem))
                            return;
                    }
                    
                    float DamageValue = damage;
                    AIDamageable.TakeDamage(DamageValue, AttackerType.AI, damager);
                    OnDamaged.Invoke();
                    
                }
                else
                {
                    AIPlayerReference PlayerReference = RootObj.GetComponent<AIPlayerReference>();
                    
                    if (PlayerReference != null)
                    {
                        float DamageValue = damage;
                        PlayerReference.TakeDamage(DamageValue, AttackerType.AI, damager);
                        OnDamaged.Invoke();
                        
                    }
                }
            } 
            if(Obj.GetComponent<Rigidbody>() != null)
            {
                Obj.GetComponent<Rigidbody>().AddForce(ImpulseForce * (Vector3.up + Vector3.forward), ForceMode.Impulse);
            }
        }
        OnExplode.Invoke();

        if (ExplosionParticle != null)
        {
            ExplosionParticle.transform.SetParent(null);
            ExplosionParticle.SetActive(true);
        }
        
        Destroy(gameObject);
    }
}
    
}