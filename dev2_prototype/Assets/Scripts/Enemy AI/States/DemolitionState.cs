namespace Zombies.AI.States
{
    public class DemolitionState : BaseAIState
    {
        public DemolitionState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.DEMOLITION;
        }

        public override void Run()
        {
            Owner.DoPhasedAttack();
        }
    }
}
