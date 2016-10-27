using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using robot.dad.combat;

namespace robot.dad.tests
{
    [TestFixture]
    public class CombatEngineTests
    {
        [Test]
        public void TestCombatEngine()
        {
            var participants = new List<Combattant>
            {
                new Combattant("Tommie", 100, 5, 5, 10, "nygren", CombatMove.CombatMoves),
                new Combattant("Lisa", 100, 5, 5, 10, "nygren", CombatMove.CombatMoves),
                new Combattant("Freja", 100, 5, 5, 10, "nygren", CombatMove.CombatMoves),
                new Combattant("Anja", 100, 5, 5, 10, "nygren", CombatMove.CombatMoves),
                new Combattant("Gargelbarg", 200, 10, -10, 20, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("GargelBite", CombatMoveType.Attack, 3, 12, 25, "bit")
                })
            };

            var ce = new CombatEngine(participants);
            ce.StartCombat();
        }
    }
}
