using System;
using System.Collections.Generic;
using robot.dad.combat;
using robot.dad.combat.MoveResolvers;

namespace robot.dad.game
{
    public class CombatDemo
    {
        public static readonly List<Combattant> Protagonists = new List<Combattant>
        {
            new Human("Tommie", "nygren", MovePickers.GetRandomPicker()),
            //new Human("Lisa", "nygren", MovePickers.RandomPicker),
            //new Human("Freja", "nygren", MovePickers.RandomPicker),
            new Human("Anja", "nygren", MovePickers.GetRandomPicker(), new List<CombatMove>()
            {
                new CombatMove("Läka sår", CombatMoveType.Healing, 10, 5, 15, "helar", Resolvers.HealingResolver)
            })
        };

        public static readonly List<Combattant> Antagonists = new List<Combattant>
        {
            new Monster("Snarfor", 30, 90, 10, 5, 10, "gargelbarg", new List<CombatMove>()
            {
                new CombatMove("Vattenförmåga", CombatMoveType.Attack, 0, 5, 10, "vattenspruta", Resolvers.AttackResolver)
            }, MovePickers.GetRandomPicker()),
            //new Monster("Gargelbarg", 200, 60, 0, 5, 10, "gargelbarg", new List<CombatMove>()
            //{
            //    new CombatMove("GargelBett", CombatMoveType.Attack, 15, 12, 25, "gargelbita", Resolvers.AttackResolver)
            //}, MovePickers.GetRandomPicker()),
            new Monster("Fyrkantsmonster", 100, 40, 30, 10, 10, "gargelbarg", new List<CombatMove>()
            {
                new CombatMove("Hypno", CombatMoveType.Special, 10, 2, 6, "hypnotisera", Resolvers.HypnosisResolver)
            }, MovePickers.GetRandomPicker())
        };

        public void StartGame()
        {
            //var ce = new CombatEngine(Protagonists, Antagonists);
            //ce.StartCombat();
            //Console.ReadKey();
        }
    }
}