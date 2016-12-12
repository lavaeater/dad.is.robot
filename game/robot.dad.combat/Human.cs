using System;
using System.Collections.Generic;
using robot.dad.combat.MoveResolvers;

namespace robot.dad.combat
{
    public class Human : Combattant
    {
        public Human(string name, string team, Action<Combattant, IEnumerable<Combattant>, List<CombatMove>> movePicker, List<CombatMove> extraMoves = null) 
            : base(name, DiceRoller.RollDice(40,70), DiceRoller.RollDice(50,75), DiceRoller.RollDice(10,20), DiceRoller.RollDice(5,10), DiceRoller.RollDice(10,20), team, HumanCombatMoves, movePicker)
        {
            if (extraMoves != null)
            {
                CombatMoves.AddRange(extraMoves);
            }
        }

        public static List<CombatMove> HumanCombatMoves => new List<CombatMove>()
        {
            new CombatMove("Slag", CombatMoveType.Attack, 10, 6, 12, "slå", Resolvers.AttackResolver),
            new CombatMove("Spark", CombatMoveType.Attack, -5, 10, 16, "sparka", Resolvers.AttackResolver),
            new CombatMove("Undvik", CombatMoveType.Defend, 20, "undvika", Resolvers.DefendResolver),
            new CombatMove("Fly", CombatMoveType.Runaway, -25, "fly", Resolvers.RunawayResolver)
        };
    }
}