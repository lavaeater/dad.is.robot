using System;
using System.Collections.Generic;

namespace robot.dad.combat
{
    public class Human : Combattant
    {
        public Human(string name, string team, Action<Combattant, List<Combattant>, List<CombatMove>> movePicker) : base(name, 50, 75, 15, 5, team, HumanCombatMoves, movePicker)
        {
        }

        public static List<CombatMove> HumanCombatMoves => new List<CombatMove>()
        {
            new CombatMove("Slag", CombatMoveType.Attack, 10, 6, 12, "slå"),
            new CombatMove("Spark", CombatMoveType.Attack, -5, 10, 16, "sparka"),
            new CombatMove("Undvik", CombatMoveType.Defend, 20, "undvika"),
            new CombatMove("Fly", CombatMoveType.Runaway, -25, "fly")
        };
    }

    public class Monster : Combattant
    {
        public Monster(string name, int health, int attackSkill, int defenseSkill, int armor, string team, List<CombatMove> combatMoves, Action<Combattant, List<Combattant>, List<CombatMove>> movePicker) : base(name, health, attackSkill, defenseSkill, armor, team, combatMoves, movePicker)
        {
        }
    }

    public class Combattant
    {
        public Combattant(string name, int health, int attackSkill, int defenseSkill, int armor, string team, List<CombatMove> combatMoves, Action<Combattant, List<Combattant>, List<CombatMove>> movePicker)
        {
            Name = name;
            Health = health;
            AttackSkill = attackSkill;
            DefenseSkill = defenseSkill;
            Armor = armor;
            Team = team;
            CombatMoves = combatMoves;
            MovePicker = movePicker;
        }

        public string Team { get; set; }
        public List<CombatMove> CombatMoves { get; set; }
        public Action<Combattant, List<Combattant>, List<CombatMove>> MovePicker { get; private set; }

        public int ApplyDamage(int damage)
        {
            int actualDamage = damage - Armor;
            actualDamage = actualDamage < 0 ? 0 : actualDamage;
            Health -= actualDamage;
            if (Health < 1)
            {
                Console.WriteLine($"{Name} dog!");
            }
            return actualDamage;
        }

        public CombatMove CurrentMove { get; set; }
        //Choose a target as well!
        public void PickMove(List<Combattant> otherTeam)
        {
            MovePicker?.Invoke(this, otherTeam, CombatMoves);
        }

        public Combattant CurrentTarget { get; set; }
        public bool Dead => Health <= 0;

        public void ClearMove()
        {
            CurrentTarget = null;
            CurrentMove = null;
        }

        public bool HasPicked => CurrentMove != null && CurrentTarget != null;
        public int Health { get; set; }
        public int DefenseSkill { get; set; }
        public int Armor { get; set; }
        public int AttackSkill { get; set; }

        public bool Npc { get; set; }
        public string Name { get; set; }
    }
}