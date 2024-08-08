namespace Zombies.AI.States
{
    public class SeekState : BaseAIState
    {
        public SeekState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.SEEK;
        }

        public override void StateBehavior()
        {
            //        free = false;
            //        FaceTarget();
            //        currentTarget = GameManager.Instance.LocalPlayer.gameObject;
            Owner.Free = false;
            Owner.FaceTarget();
            Owner.currentTarget = GameManager.Instance.LocalPlayer.gameObject;

            Owner.StartCoroutine(Owner.TargetProximityCheck(0.1f));
            Owner.TargetVisibilityCheck();
        }
    }
}
