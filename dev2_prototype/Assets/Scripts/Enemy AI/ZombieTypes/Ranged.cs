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
        TargetVisibilityCheck();
        StartCoroutine(TargetProximityCheck());
        if (nearPlayer && seesTarget)
        {
            State = enemyState.ATTACK;
        }
    }

    public override void Attack()
    {
        if (State == enemyState.DEAD)
            return;

        UpdateTargetDir();
        FaceTarget();

        if (attacking)
        {
            Attacking();
            return;
        }

        fleeing = GetDistanceToTarget() < fleeingDist;
        if (fleeing)
        {
            State = enemyState.FLEE;
            return;
        }

        TargetVisibilityCheck();

        if (!seesTarget && !fleeing)
        {
            State = enemyState.SEEK;
            return;
        }

        Attacking();
    }

    public void OnAttackHit()
    {
        if (currentTarget.tag.Contains("Barricade"))
        {
            GameObject barrChild = currentTarget.gameObject.transform.GetChild(0).gameObject;

            if (barrChild.TryGetComponent(out IDamageable dmg))
                dmg.TakeDamage(destructionPower);
            if (!barrChild.activeSelf)
            {
                State = enemyState.SEEK;
                phase = attackPhase.IDLE;
            }
                
            return;
        }
        AttackLogic();
    }

    protected override void AttackLogic()
    {
        UpdateTargetDir();

        Weapon.Info.Damage = AttackDMG;
        Weapon.Info.FireRate = fireRate;
        Weapon.Shoot(ShootPos.position, targetDir);
    }
}