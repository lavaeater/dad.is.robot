using System;
using System.Collections.Generic;
using robot.dad.combat;

namespace robot.dad.game
{
    class Program
    {
        static void Main(string[] args)
        {
            var participants = new List<Combattant>
            {
                new Combattant("Tommie", 100, 5, 5, 3, "nygren", CombatMove.CombatMoves),
                new Combattant("Lisa", 100, 5, 5, 3, "nygren", CombatMove.CombatMoves),
                new Combattant("Freja", 100, 5, 5, 3, "nygren", CombatMove.CombatMoves),
                new Combattant("Dante", 100, 5, 5, 3, "nygren", CombatMove.CombatMoves),
                new Combattant("Gargelbarg", 200, 10, -10, 5, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("GargelBett", CombatMoveType.Attack, 3, 12, 25, "bet")
                })
            };

            var ce = new CombatEngine(participants);
            ce.StartCombat();
            Console.ReadKey();
            //var game = new Game("dad is robot", 1600,900,60, true);
            //game.Start();
        }
    }
}
