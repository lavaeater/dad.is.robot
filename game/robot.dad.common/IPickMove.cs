using System.Collections.Generic;

namespace robot.dad.common
{
    public interface IPickMove
    {
        void PickMove(ICombattant attacker, IEnumerable<ICombattant> possibleTargets);
    }
}