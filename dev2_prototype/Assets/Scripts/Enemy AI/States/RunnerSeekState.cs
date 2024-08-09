namespace Zombies.AI.States
{
    public class RunnerSeekState : SeekState
    {
        public RunnerSeekState(BaseZombie owner) : base(owner) { }

        public override void Run()
        {
            // Call the base class's behavior
            base.Run();

            Owner.Move();

            Owner.Agent.speed = Owner.SeesTarget ? Owner.MovementSpeed * 2 : Owner.MovementSpeed;
    
            if (Owner.NearTarget)
            {
                Owner.UpdateState(new RunnerAttackState(Owner));
            }
        }
    }
}