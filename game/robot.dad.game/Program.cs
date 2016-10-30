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
                new Human("Tommie", "nygren", MovePickers.RandomPicker),
                new Human("Lisa", "nygren", MovePickers.RandomPicker),
                new Human("Freja", "nygren", MovePickers.RandomPicker),
                new Human("Anja", "nygren", MovePickers.RandomPicker),
                new Monster("Snarfor", 30, 90, 10, 5, "nygren", new List<CombatMove>()
                {
                    new CombatMove("Vattenförmåga", CombatMoveType.Attack, 0, 5, 10, "vattenspruta")
                }, MovePickers.RandomPicker),
                new Monster("Gargelbarg", 200, 60, 0, 5, "gargelbarg", new List<CombatMove>()
                {
                    new CombatMove("GargelBett", CombatMoveType.Attack, 15, 12, 25, "gargelbita")
                }, MovePickers.RandomPicker),
                new Monster("Fyrkantsmonster", 100, 80, 30, 10, "gargelbarg", new List<CombatMove>()
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
