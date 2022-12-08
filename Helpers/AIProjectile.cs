using System;
using UnityEngine;


namespace AI
{
public class AIProjectile : MonoBehaviour
{

    public AIEnums.YesNo DebugHitObject = AIEnums.YesNo.No;
    [HideInInspector] public float damage;
    [HideInInspector] public AIShooterWeapon weapon;
    [HideInInspector] public GameObject damager;
    [HideInInspector] public LayerMask ProjectileHitLayers = 1;

    private bool canthit = false;
    private void Update()
    {
      if(canthit)
          return;
      
      RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.9f, ProjectileHitLayers,
                QueryTriggerInteraction.Ignore))
        {
            GameObject col = hit.transform.gameObject;
            if (DebugHitObject == AIEnums.YesNo.Yes)
            {
                Debug.Log("Projectile Hit: " + col.gameObject.name + " !!");
            }
        
            if (col.GetComponent<AIPlayerReference>() != null)
            {
                col.GetComponent<AIPlayerReference>().TakeDamage(damage, AttackerType.AI, damager);
            }

            if (col.GetComponent<AIDamageable>() != null)
            {
                if (col.GetComponent<AIDamageable>() is AISystem)
                {
                    if (col.GetComponent<AISystem>() == weapon.AISystem)
                    {
                        return;
                    }

                    // if (col.GetComponent<AISystem>().Detection.Factions.Factions
                    //     .Equals(weapon.AISystem.Detection.Factions.Factions))
                    // {
                    //     return;
                    // }
                }
                else if (col.GetComponent<AIDamageable>() is AIDamageableObject)
                {
                    if (col.GetComponent<AIDamageableObject>().AISystem == weapon.AISystem)
                    {
                        return;
                    }
                }
                col.GetComponent<AIDamageable>().TakeDamage(damage, AttackerType.AI, damager);
                weapon.AISystem.AIEvents.OnDealDamage.Invoke(damage);
                canthit = true;
            }

            if (weapon.VFXSettings.BulletSettings.ImpactEffects.Count > 0)
            {
                if (weapon.VFXSettings.BulletSettings.ImpactEffects.Count < 2)
                {
                    GameObject Effect = Instantiate(weapon.VFXSettings.BulletSettings.ImpactEffects[0].ImpactEffect,
                        hit.point + new Vector3(0.0f,0,0.0f), Quaternion.FromToRotation(Vector3.forward, hit.normal));

                    if (weapon.VFXSettings.BulletSettings.ImpactEffects[0].SetParent == AIMeleeWeapon.impactList.setParent.Null)
                    {
                        Effect.transform.SetParent(null);
                    }
                    else
                    {
                        Effect.transform.SetParent(hit.transform);
                    }
                }
                else
                {
                    foreach (var effect in weapon.VFXSettings.BulletSettings.ImpactEffects)
                    {
                        if (hit.transform.tag.Equals(effect.ObjectTag))
                        {
                            GameObject Effect = Instantiate(effect.ImpactEffect,
                                hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

                            if (effect.SetParent == AIMeleeWeapon.impactList.setParent.Null)
                            {
                                Effect.transform.SetParent(null);
                            }
                            else
                            {
                                Effect.transform.SetParent(hit.transform);
                            }
                            
                            break;
                        }
                    }   
                }
            }
            Destroy(gameObject, 0.07f);
        }
    }

}
}