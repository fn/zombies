using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Zombies.AI.States
{
    public class DeadState : BaseAIState
    {
        public DeadState(BaseZombie owner) : base(owner)
        {
            Name = EnemyState.DEAD;
        }

        public override void Run()
        {
            Owner.ResetAttack();
            Owner.Free = false;
            Owner.Agent.ResetPath();

            if (Owner.animator != null)
                Owner.animator.SetBool("Dead", true);
        }
    }
}
