using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zombies.AI;
using Zombies.AI.States;

public class Ranged : BaseZombie
{
    [SerializeField] WeaponComponent Weapon;
    [SerializeField] Transform ShootPos;

    [SerializeField] float fireRate;
    [SerializeField] float fleeingDist;

    public float FleeingDistance { get => fleeingDist; }

    // public override void Seek()
    // {
    //     agent.speed = movementSpeed;
    //     Move();
    //     TargetVisibilityCheck();
    //     StartCoroutine(TargetProximityCheck());
    //     if (nearPlayer && seesTarget)
    //     {
    //         State = enemyState.ATTACK;
    //     }
    // }

    //public override void Attack()
    //{

    //}

    public void OnAttackHit()
    {
        if (CurrentTarget.tag.Contains("Barricade"))
        {
            if(CurrentTarget.gameObject.TryGetComponent(out Barricade barricade))
            {
                barricade.TakeDamage(destructionPower);

                if (barricade.IsBroken)
                {
                    UpdateState(GetSeekState());

                    ResetAttack();
                }

            }

            //GameObject barrChild = CurrentTarget.gameObject.transform.GetChild(0).gameObject;

            //if (barrChild.TryGetComponent(out IDamageable dmg))
            //    dmg.TakeDamage(destructionPower);
            //if (!barrChild.activeSelf)
            //{
            //    // State = enemyState.SEEK;
            //    UpdateState(GetSeekState());

            //    AttackPhase = AttackPhases.IDLE;
            //}

            return;
        }

        AttackLogic();
    }

    protected override void AttackLogic()
    {
        UpdateTargetDir();

        Weapon.Info.Damage = AttackDamage;
        Weapon.Info.FireRate = fireRate;
        Weapon.Shoot(ShootPos.position, targetDir);
    }

    public override BaseAIState GetNormalState()
    {
        return null;
    }

    public override BaseAIState GetAttackState()
    {
        return new RangedAttackState(this);
    }

    public override BaseAIState GetSeekState()
    {
        return new RangedSeekState(this);
    }
}