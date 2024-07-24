using System.Reflection;
using UnityEngine;

public class ZombieAnimationProxy : MonoBehaviour
{
    [SerializeField] Ranged RangedZombie;

    // Start is called before the first frame update
    void OnAttackHit()
    {
        RangedZombie.OnAttackHit();
    }

    void AttackLogic()
    {
        typeof(Ranged).GetMethod(nameof(AttackLogic), BindingFlags.NonPublic | BindingFlags.Instance).Invoke(RangedZombie, null);
    }
}