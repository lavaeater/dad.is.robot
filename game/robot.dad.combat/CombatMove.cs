using System;
using System.Collections.Generic;

namespace robot.dad.combat
{
    public class CombatMove
    {
        public CombatMove(string name, CombatMoveType moveType, int modifier, int minDamage, int maxDamage, string verbified, Action<CombatMove, Combattant, Combattant> applyMove)
        {
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Verbified = verbified;
            ApplyMove = applyMove;
        }

        public CombatMove(string name, CombatMoveType moveType, int modifier, string verbified, Action<CombatMove, Combattant, Combattant> moveApplier)
        {
            if (moveType == CombatMoveType.Attack)
            {
                throw new InvalidOperationException("Attack move types MUST have min and max damage");
            }
            Name = name;
            MoveType = moveType;
            Modifier = modifier;
            Verbified = verbified;
            ApplyMove = moveApplier;
        }

        public void Apply(Combattant attacker, Combattant target)
        {
            ApplyMove.Invoke(this, attacker, target);
        }

        public string Name { get; set; }
        public CombatMoveType MoveType { get; set; }
        public int Modifier { get; set; }
        public int MaxDamage { get; set; }
        public int MinDamage { get; set; }
        public string Verbified { get; set; }
        public Action<CombatMove, Combattant, Combattant> ApplyMove { get; set; }
    }

    public static class CombatMoveAppliers
    {
        public static Action<CombatMove, Combattant, Combattant> DamageApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            if (target.CurrentMove.MoveType == CombatMoveType.Defend ||
                target.CurrentMove.MoveType == CombatMoveType.Runaway)
            {
                targetValue += target.CurrentMove.Modifier;
            }

            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                int damageRoll = diceRoll <= 5 ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage);
                Console.WriteLine($"och sl�r {diceRoll}, tr�ffar och g�r {target.ApplyDamage(damageRoll)} i skada!");
                Console.WriteLine($"{target.Name} har {target.Health} kvar i h�lsa.");
            }
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> HealingApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.AttackSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {               
                //1 == perfekt slag!
                int damageRoll = diceRoll <= 5 ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage);
                target.Health += damageRoll;
                Console.WriteLine($"och sl�r {diceRoll}, lyckas och helar {damageRoll} h�lsa!");
                Console.WriteLine($"{target.Name} har {target.Health} kvar i h�lsa.");
            }
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> HypnosisApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.AttackSkill + move.Modifier - target.DefenseSkill;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} {target.Name} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                int damageRoll = diceRoll <= 5 ? move.MaxDamage : DiceRoller.RollDice(move.MinDamage, move.MaxDamage);
                Console.WriteLine($"och sl�r {diceRoll}, tr�ffar och {move.Verbified} {target.Name}!");
            }
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> RunawayApplier = (move, attacker, target) =>
        {
            int targetValue = attacker.DefenseSkill + move.Modifier;
            int diceRoll = DiceRoller.RollHundredSided();
            Console.Write($"{attacker.Name} m�ste sl� under {targetValue} f�r att {move.Verbified} - ");
            if (diceRoll <= targetValue)
            {
                //1 == perfekt slag!
                //vad g�r perfekt slag och hur?
                Console.WriteLine($"och sl�r {diceRoll} och {move.Verbified}!");
                attacker.Runaway();
            }
            else
            {
                //100 == perfekt fail! Vad h�nder? N�t kul!
                Console.WriteLine($"men sl�r {diceRoll} och missar!");
            }
        };

        public static Action<CombatMove, Combattant, Combattant> DefendApplier = (move, attacker, target) =>
        {
            Console.WriteLine($"{attacker.Name} {move.Verbified}");
        };
    }

}