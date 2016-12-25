using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.dad.combat.MoveResolvers
{
    public class ResolveDefendMove : ResolveMoveBase
    {
        public override bool ResolveMove(ICombatMove move, ICombattant attacker, ICombattant target)
        {
            return true;
        }
    }
}
