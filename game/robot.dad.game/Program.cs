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
                new Combattant("Tommie", 50, 55, 20, 3, "nygren", CombatMove.CombatMoves, MovePickers.ManualPicker),
                new Combattant("Lisa", 50, 65, 15, 3, "nygren", CombatMove.CombatMoves, MovePickers.RandomPicker),
                new Combattant("Freja", 50, 75, 20, 3, "nygren", CombatMove.CombatMoves, MovePickers.RandomPicker),
                new Combattant("Dante", 50, 70, 25, 3, "nygren", CombatMove.CombatMoves, MovePickers.RandomPicker),
                new Combattant("Snarfor", 30, 90, 10, 5, "nygren", new List<CombatMove>()
                {
                    new CombatMove("Vattenförmåga", CombatMoveType.Attack, 0, 5, 10, "vattenspruta")
                }, MovePickers.RandomPicker),
                new Combattant("Gargelbarg", 200, 60, 0, 5, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("GargelBett", CombatMoveType.Attack, 15, 12, 25, "gargelbita")
                }, MovePickers.RandomPicker),
                new Combattant("Fyrkantsmonster", 100, 80, 30, 10, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("Hypno", CombatMoveType.Special, -10, "hypnotisera")
                }, MovePickers.RandomPicker)
            };

            var ce = new CombatEngine(participants);
            ce.StartCombat();
            Console.ReadKey();
            //var game = new Game("dad is robot", 1600,900,60, true);
            //game.Start();
        }
    }
}
