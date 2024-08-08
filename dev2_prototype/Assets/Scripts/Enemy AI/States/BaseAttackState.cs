namespace Zombies.AI.States
{
    public class BaseAttackState : BaseAIState
    {
        public BaseAttackState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.ATTACK;
        }

        public override void StateBehavior()
        {
            Owner.Move();
            Owner.StartCoroutine(Owner.TargetProximityCheck(0.1f));
            Owner.TargetVisibilityCheck();
            Owner.Agent.speed = Owner.SeesTarget ? Owner.MoveSPD * 2 : Owner.MoveSPD;

            if (Owner.IsAttacking)
            {
                return;
            }
            if (Owner.NearTarget)
            {
                Owner.UpdateState(Owner.GetAttackState());
            }
        }
    }
}