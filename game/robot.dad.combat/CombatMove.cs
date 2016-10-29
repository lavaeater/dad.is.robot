using System;
using System.Collections.Generic;

namespace robot.dad.combat
{
    public class CombatMove
    {
        public CombatMove(string name, CombatMoveType moveType, int modifier, int minDamage, int maxDamage, string verbified)
        {
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Verbified = verbified;
        }

        public CombatMove(string name, CombatMoveType moveType, int modifier, string verbified)
        {
            if (moveType == CombatMoveType.Attack)
            {
                throw new InvalidOperationException("Attack move types MUST have min and max damage");
            }
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            Verbified = verbified;
        }


        public string Name { get; set; }
        public CombatMoveType MoveType { get; set; }
        public int Modifier { get; set; }
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public static List<CombatMove> CombatMoves => new List<CombatMove>()
        {
            new CombatMove("Slag", CombatMoveType.Attack, 10, 6, 12, "slå"),
            new CombatMove("Spark", CombatMoveType.Attack, -5, 10, 16, "sparka"),
            new CombatMove("Undvik", CombatMoveType.Defend, 20, "undvika"),
            new CombatMove("Fly", CombatMoveType.Runaway, -25, "fly")
        };

        public string Verbified { get; set; }
    }
}