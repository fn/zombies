using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : BaseZombie
{
    public override void Seek()
    {
        Move();

        StartCoroutine(TargetCheck(0.1f));
        VisibilityCheck();

        agent.speed = SeesPlayer ? movementSpeed * 2 : movementSpeed;

        if (attacking)
        {
            return;
        }
        if (nearPlayer)
        {
            State = enemyState.ATTACK;
        }
    }

    public override void Attack()
    {
        Attacking();
    }

    protected override void AttackLogic()
    {
        if (nearPlayer)
        {
            if (targetPlayer.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(AttackDMG);
            }
        }
    }
}