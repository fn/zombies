namespace Zombies.AI.States
{
    public class RunnerSeekState : SeekState
    {
        public RunnerSeekState(BaseZombie owner) : base(owner) { }

        public override void StateBehavior()
        {
            // Call the base class's behavior
            base.StateBehavior();

            Owner.Move();

            Owner.Agent.speed = Owner.SeesTarget ? Owner.MoveSPD * 2 : Owner.MoveSPD;

            if (Owner.NearTarget)
            {
                Owner.UpdateState(new RunnerAttackState(Owner));
            }
        }
    }
}