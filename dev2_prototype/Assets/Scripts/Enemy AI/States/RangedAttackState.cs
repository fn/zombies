namespace Zombies.AI.States
{
    public class RangedAttackState : BaseAIState
    {
        public RangedAttackState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.ATTACK;
        }

        public override void Run()
        {
            Owner.UpdateTargetDir();
            Owner.FaceTarget();

            if (Owner.IsAttacking)
            {
                Owner.DoPhasedAttack();
                return;
            }

            if (Owner is Ranged)
            {
                var ranged = Owner as Ranged;

                ranged.IsFleeing = ranged.GetDistanceToTarget() < ranged.FleeingDistance;

                if (ranged.IsFleeing)
                {
                    Owner.UpdateState(new FleeingState(Owner));
                }

                Owner.TargetVisibilityCheck();

                // We couldn't find a target.
                if (!ranged.SeesTarget && !ranged.IsFleeing)
                {
                    Owner.UpdateState(Owner.GetSeekState());
                    return;
                }
            }

            // Advance one phase in attack phase.
            Owner.DoPhasedAttack();
        }
    }
}