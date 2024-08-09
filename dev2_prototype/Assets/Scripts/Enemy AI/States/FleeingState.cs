using UnityEngine;

namespace Zombies.AI.States
{
    public class FleeingState : BaseAIState
    {
        public FleeingState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.FLEE;
        }

        public override void Run()
        {
            Owner.Agent.speed = 2f * Owner.MovementSpeed;
            Owner.UpdateTargetDir();

            Vector3 newPos = (Owner.transform.position - Owner.targetDir);
            //Debug.Log("New:" + newPos);
            //Debug.Log("Old: " + transform.position);
            Owner.Agent.stoppingDistance = 0;
            Owner.Agent.SetDestination(newPos);
            if (Owner.GetDistanceToTarget() >= Owner.detectionRange)
            {
                Owner.IsFleeing = false;
                Owner.UpdateState(Owner.GetSeekState());
            }
        }
    }
}
