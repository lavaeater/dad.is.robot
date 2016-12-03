using System;
using System.Collections.Generic;
using robot.dad.combat;
using robot.dad.combat.MoveResolvers;

namespace robot.dad.game
{
    public class CombatDemo
    {

        List<Combattant> _participants = new List<Combattant>
        {
            new Human("Tommie", "nygren", MovePickers.RandomPicker),
            new Human("Lisa", "nygren", MovePickers.RandomPicker),
            new Human("Freja", "nygren", MovePickers.RandomPicker),
            new Human("Anja", "nygren", MovePickers.RandomPicker, new List<CombatMove>()
            {
                new CombatMove("L�ka s�r", CombatMoveType.Healing, 10, 5, 15, "helar", Resolvers.HealingResolver)
            }),
            new Monster("Snarfor", 30, 90, 10, 5, "nygren", new List<CombatMove>()
            {
                new CombatMove("Vattenf�rm�ga", CombatMoveType.Attack, 0, 5, 10, "vattenspruta", Resolvers.AttackResolver)
            }, MovePickers.RandomPicker),
            new Monster("Gargelbarg", 200, 60, 0, 5, "gargelbarg", new List<CombatMove>()
            {
                new CombatMove("GargelBett", CombatMoveType.Attack, 15, 12, 25, "gargelbita", Resolvers.AttackResolver)
            }, MovePickers.RandomPicker),
            new Monster("Fyrkantsmonster", 100, 40, 30, 10, "gargelbarg", new List<CombatMove>()
            {
                new CombatMove("Hypno", CombatMoveType.Special, -10, "hypnotisera", Resolvers.HypnosisResolver)
            }, MovePickers.RandomPicker)
        };

        public void StartGame()
        {
            var ce = new CombatEngine(_participants);
            ce.StartCombat();
            Console.ReadKey();
        }
    }
}