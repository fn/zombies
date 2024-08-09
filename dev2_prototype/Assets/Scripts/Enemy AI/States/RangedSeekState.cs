using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombies.AI.States
{
    public class RangedSeekState : SeekState
    {
        public RangedSeekState(BaseZombie owner) : base(owner) { }

        public override void Run()
        {
            base.Run();

            Owner.Move();
            Owner.Agent.speed = Owner.MovementSpeed;

            if (Owner.NearTarget && Owner.SeesTarget)
                Owner.UpdateState(new RangedAttackState(Owner));
        }
    }
}
