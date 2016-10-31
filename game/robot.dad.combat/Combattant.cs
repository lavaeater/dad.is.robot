using System;
using System.Collections.Generic;
using System.Text;

namespace robot.dad.combat
{
    public class Human : Combattant
    {
        public Human(string name, string team, Action<Combattant, List<Combattant>, List<CombatMove>> movePicker, List<CombatMove> extraMoves = null) : base(name, 50, 75, 15, 5, team, HumanCombatMoves, movePicker)
        {
            if (extraMoves != null)
            {
                CombatMoves.AddRange(extraMoves);
            }
        }

        public static List<CombatMove> HumanCombatMoves => new List<CombatMove>()
        {
            new CombatMove("Slag", CombatMoveType.Attack, 10, 6, 12, "slå", CombatMoveAppliers.DamageApplier),
            new CombatMove("Spark", CombatMoveType.Attack, -5, 10, 16, "sparka", CombatMoveAppliers.DamageApplier),
            new CombatMove("Undvik", CombatMoveType.Defend, 20, "undvika", CombatMoveAppliers.DefendApplier),
            new CombatMove("Fly", CombatMoveType.Runaway, -25, "fly", CombatMoveAppliers.RunawayApplier)
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
            CurrentHealth = Health;
            AttackSkill = attackSkill;
            DefenseSkill = defenseSkill;
            Armor = armor;
            Team = team;
            CombatMoves = combatMoves;
            MovePicker = movePicker;
            Status = CombatStatus.Active;
        }

        public void ApplyMove()
        {
            CurrentMove.Apply(this, CurrentTarget);
        }

        public string Team { get; set; }
        public List<CombatMove> CombatMoves { get; set; }
        public Action<Combattant, List<Combattant>, List<CombatMove>> MovePicker { get; private set; }

        public int ApplyDamage(int damage)
        {
            int actualDamage = damage - Armor;
            actualDamage = actualDamage < 0 ? 0 : actualDamage;
            CurrentHealth -= actualDamage;
            if (CurrentHealth < 1)
            {
                Die();
                Console.WriteLine($"{Name} dog!");
            }
            return actualDamage;
        }

        public CombatStatus Status { get; set; }

        public void Die()
        {
            Status = CombatStatus.Dead;
        }

        public void Runaway()
        {
            Status = CombatStatus.Fled;
        }

        public CombatMove CurrentMove { get; set; }
        //Choose a target as well!
        public void PickMove(List<Combattant> otherTeam)
        {
            MovePicker?.Invoke(this, otherTeam, CombatMoves);
        }

        public Combattant CurrentTarget { get; set; }
        public bool Dead => CurrentHealth <= 0;

        public void ClearMove()
        {
            CurrentTarget = null;
            CurrentMove = null;
        }
        
        public bool HasPicked => CurrentMove != null && CurrentTarget != null;
        public int Health { get; set; }
        public int CurrentHealth { get; set; }
        public int DefenseSkill { get; set; }
        public int Armor { get; set; }
        public int AttackSkill { get; set; }

        public bool Npc { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name}");
            sb.AppendLine($"Hälsa: {CurrentHealth} / {Health}");
            sb.AppendLine($"Status: {Status}");
            return sb.ToString();
        }
    }

    public enum CombatStatus
    {
        Active,
        Fled,
        Dead,
        Inactive
    }
}