namespace Zombies.AI.States
{
    public class SeekState : BaseAIState
    {
        public SeekState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.SEEK;
        }

        public override void Run()
        {
            Owner.Free = false;
            Owner.FaceTarget();
            Owner.CurrentTarget = GameManager.Instance.LocalPlayer.gameObject;

            Owner.TargetProximityCheck(0.1f);
            Owner.TargetVisibilityCheck();
        }
    }
}
