namespace Zombies.AI
{
    public abstract class BaseAIState
    {
        public BaseAIState(BaseZombie owner)
        {
            Owner = owner;
        }

        public BaseZombie Owner;

        // The current state we are in.
        public EnemyState Name;

        // The behavior of the current state.
        public abstract void Run();
    }
}