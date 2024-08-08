
namespace Zombies.AI.States
{
    public class CommanderNormalState : BaseAIState
    {
        public CommanderNormalState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.NORMAL;
        }

        public override void StateBehavior()
        {
            Owner.UpdateTargetDir();
            Owner.FaceTarget();
            Owner.TargetVisibilityCheck();
            if (Owner.SeesTarget)
            {
                Owner.UpdateState(Owner.GetAttackState());
                return;
            }

            var commander = Owner as Commander;

            //if (commander.readyZombies >= commander.mainGroup.Count / 2)
            //{
            //    Owner.UpdateState(FlankState());

            //    State = enemyState.FLANK;
            //    readyZombies = 0;
            //}
        }
    }
}
