using UnityEngine;

namespace Zombies.AI.States
{
    public class RunnerAttackState : BaseAIState
    {
        public RunnerAttackState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.ATTACK;
        }

        public override void Run()
        {
            Owner.DoPhasedAttack();
        }
    }
}
