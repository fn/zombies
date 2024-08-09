using UnityEngine;

namespace Zombies.AI.States
{
    public class FlankState : BaseAIState
    {
        public FlankState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.FLANK;
        }

        public override void Run()
        {
            Owner.Agent.speed = 2 * Owner.MovementSpeed;
            Owner.FlankTarget(Owner.CurrentTarget.transform, Owner.commander.flankingDeviation);
            if (Vector3.Distance(Owner.transform.position, Owner.CurrentTarget.transform.position) <= Owner.Agent.stoppingDistance + Owner.commander.flankingDeviation)
            {
                // Set seek state.
                Owner.UpdateState(Owner.GetSeekState());
            }
        }
    }
}
