using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : BaseZombie
{
    [SerializeField] WeaponComponent Weapon;
    [SerializeField] Transform ShootPos;

    [SerializeField] float fireRate;
    [SerializeField] float fleeingDist;

    public override void Seek()
    {
        agent.speed = movementSpeed;
        Move();
        VisibilityCheck();
        StartCoroutine(TargetCheck());
        if (nearPlayer && seesPlayer)
        {
            State = enemyState.ATTACK;
        }
    }

    public override void Attack()
    {
        UpdatePlayerDir();
        FaceTarget();

        if (attacking)
        {
            Attacking();
            return;
        }

        fleeing = GetDistanceToPlayer() < fleeingDist;
        if (fleeing)
        {
            State = enemyState.FLEE;
            return;
        }

        VisibilityCheck();

        if (!seesPlayer && !fleeing)
        {
            State = enemyState.SEEK;
            return;
        }

        Attacking();
    }

    void OnAttackHit()
    {
        if (currentTarget.tag.Contains("Barricade"))
        {
            GameObject barrChild = currentTarget.gameObject.transform.GetChild(0).gameObject;

            if (barrChild.TryGetComponent(out IDamageable dmg))
                dmg.TakeDamage(destructionPower);
            if (!barrChild.activeSelf)
                State = enemyState.ATTACK;
            return;
        }
        AttackLogic();
    }


    protected override void AttackLogic()
    {
        UpdatePlayerDir();

        Weapon.Info.Damage = AttackDMG;
        Weapon.Info.FireRate = fireRate;
        Weapon.Shoot(ShootPos.position, playerDir);
    }
}