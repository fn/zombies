namespace Zombies.AI.States
{
    public class BaseAttackState : BaseAIState
    {
        public BaseAttackState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.ATTACK;
        }

        public override void Run()
        {
            Owner.Move();
            Owner.TargetProximityCheck(0.1f);
            Owner.TargetVisibilityCheck();
            Owner.Agent.speed = Owner.SeesTarget ? Owner.MovementSpeed * 2 : Owner.MovementSpeed;

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