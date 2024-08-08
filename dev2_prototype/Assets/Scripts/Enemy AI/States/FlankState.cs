using UnityEngine;

namespace Zombies.AI.States
{
    public class FlankState : BaseAIState
    {
        public FlankState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.FLANK;
        }

        public override void StateBehavior()
        {
            Owner.Agent.speed = 2 * Owner.MoveSPD;
            Owner.FlankTarget(Owner.currentTarget.transform, Owner.commander.flankingDeviation);
            if (Vector3.Distance(Owner.transform.position, Owner.currentTarget.transform.position) <= Owner.Agent.stoppingDistance + Owner.commander.flankingDeviation)
            {
                // Set seek state.
                Owner.UpdateState(Owner.GetSeekState());
            }
        }
    }
}
