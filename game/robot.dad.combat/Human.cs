using System;
using System.Collections.Generic;
using robot.dad.combat.MoveResolvers;

namespace robot.dad.combat
{
    public class Human : Combattant
    {
        public Human(string name, string team, IPickMove movePicker, List<ICombatMove> extraMoves = null) 
            : base(name, DiceRoller.RollDice(40,70), DiceRoller.RollDice(50,75), DiceRoller.RollDice(10,20), DiceRoller.RollDice(5,10), DiceRoller.RollDice(10,20), team, HumanCombatMoves, movePicker)
        {
            if (extraMoves != null)
            {
                CombatMoves.AddRange(extraMoves);
            }
        }

        public static List<ICombatMove> HumanCombatMoves => new List<ICombatMove>()
        {
            new CombatMove("Skjuta", CombatMoveType.Attack, 10, 20, 40, "skjuter", Resolvers.AttackResolver),
            new CombatMove("Spark", CombatMoveType.Attack, 0, 10, 16, "sparkar", Resolvers.AttackResolver),
            new CombatMove("Undvik", CombatMoveType.Defend, 20,0,0, "undviker", Resolvers.DefendResolver),
            new CombatMove("Fly", CombatMoveType.Runaway, -5,0,0, "flyr", Resolvers.RunawayResolver)
        };
    }
}