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
            var participants = new List<Combattant>();
            participants.Add(new Combattant()
            {
                Npc = false
            });
            participants.Add(new Combattant()
            {
                Npc = true
            });


            var ce = new CombatEngine(participants);
            ce.StartCombat();
        }
    }
}
