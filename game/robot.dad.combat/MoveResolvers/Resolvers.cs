using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot.dad.combat.MoveResolvers
{
    public static class Resolvers
    {
        public static ResolveAttackMove AttackResolver => new ResolveAttackMove();
        public static ResolveHealingMove HealingResolver => new ResolveHealingMove();
        public static ResolveHypnosisMove HypnosisResolver => new ResolveHypnosisMove();
        public static ResolveRunawayMove RunawayResolver => new ResolveRunawayMove();
        public static ResolveDefendMove DefendResolver => new ResolveDefendMove();
    }
}
