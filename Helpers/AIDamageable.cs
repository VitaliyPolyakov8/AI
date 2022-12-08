using UnityEngine;

namespace AI
{

    public enum AttackerType
    {
        Player,
        AI,
        Other,
    }
    public enum AttackerTypeCheck
    {
        All,
        Player,
        AI,
        Other,
    }
public interface AIDamageable
{
    void TakeDamage(float damageamount, AttackerType attackerType, GameObject Attacker, bool BlockSuccess = true);
}

}