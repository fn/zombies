using UnityEngine;
using Zombies.AI;
using Zombies.AI.States;

public class Runner : BaseZombie
{
    //public override void Attack()
    //{
    //    Attacking();
    //}

    public void OnAttackHit()
    {
        AttackLogic();
    }

    protected override void AttackLogic()
    {
        nearPlayer = GetDistanceToTarget() <= origStoppingDistance;
        TargetVisibilityCheck();
        if (CurrentTarget.tag.Contains("Barricade"))
        {
            GameObject barrChild = CurrentTarget.gameObject.transform.GetChild(0).gameObject;

            if (barrChild.TryGetComponent(out IDamageable dmg))
                dmg.TakeDamage(destructionPower);
            if (!barrChild.activeSelf)
            {
                var attackState = GetAttackState();
                if (attackState != null)
                {
                    UpdateState(attackState);
                    AttackPhase = AttackPhases.RECOVERY;
                }
            }

            return;
        }
        else if (CurrentTarget.TryGetComponent(out IDamageable dmg))
        {

            if (!nearPlayer || !seesTarget)
                return;
            dmg.TakeDamage(AttackDamage);
        }      
    }

    public override BaseAIState GetNormalState()
    {
        throw new System.NotImplementedException();
    }

    public override BaseAIState GetAttackState()
    {
        return new RunnerAttackState(this);
    }

    public override BaseAIState GetSeekState()
    {
        return new RunnerSeekState(this);
    }
}