using System;
using System.Collections.Generic;
using robot.dad.combat.MoveResolvers;

namespace robot.dad.combat
{
    public class Human : Combattant
    {
        public Human(string name, string team, IPickMoves movePicker, List<CombatMove> extraMoves = null) 
            : base(name, DiceRoller.RollDice(40,70), DiceRoller.RollDice(50,75), DiceRoller.RollDice(10,20), DiceRoller.RollDice(5,10), DiceRoller.RollDice(10,20), team, HumanCombatMoves, movePicker)
        {
            if (extraMoves != null)
            {
                CombatMoves.AddRange(extraMoves);
            }
        }

        public static List<CombatMove> HumanCombatMoves => new List<CombatMove>()
        {
            new CombatMove("Skjuta", CombatMoveType.Attack, 10, 20, 40, "skjuta", Resolvers.AttackResolver),
            new CombatMove("Spark", CombatMoveType.Attack, 0, 10, 16, "sparka", Resolvers.AttackResolver),
            new CombatMove("Undvik", CombatMoveType.Defend, 20, "undvika", Resolvers.DefendResolver),
            new CombatMove("Fly", CombatMoveType.Runaway, -5, "fly", Resolvers.RunawayResolver)
        };
    }
}