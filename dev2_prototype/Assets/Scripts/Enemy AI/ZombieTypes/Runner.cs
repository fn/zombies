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

    public void OnAttackHit()
    {
        AttackLogic();
    }

    protected override void AttackLogic()
    {
        nearPlayer = GetDistanceToPlayer() <= origStoppingDistance;
        if (currentTarget.tag.Contains("Barricade"))
        {
            GameObject barrChild = currentTarget.gameObject.transform.GetChild(0).gameObject;

            if (barrChild.TryGetComponent(out IDamageable dmg))
                dmg.TakeDamage(destructionPower);
            if (!barrChild.activeSelf)
                State = enemyState.ATTACK;
            return;
        }
        else if (targetPlayer.TryGetComponent(out IDamageable dmg))
        {
            // Sees player seems to be false a lot I don't know lets get hit through walls instead...
            if (!nearPlayer /*|| !seesPlayer*/)
                return;
            dmg.TakeDamage(AttackDMG);
        }      
    }
}