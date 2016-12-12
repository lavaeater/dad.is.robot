using System;
using System.Collections.Generic;
using System.Text;
using robot.dad.combat.Interfaces;

namespace robot.dad.combat
{
    public class Combattant
    {
        public Combattant(string name, int health, int attackSkill, int defenseSkill, int armor, int initiative, string team, List<CombatMove> combatMoves, Action<Combattant, IEnumerable<Combattant>, List<CombatMove>> movePicker)
        {
            Name = name;
            Health = health;
            CurrentHealth = Health;
            AttackSkill = attackSkill;
            DefenseSkill = defenseSkill;
            Armor = armor;
            Initiative = initiative;
            Team = team;
            CombatMoves = combatMoves;
            MovePicker = movePicker;
            Status = CombatStatus.Active;
        }

        public void ResolveMove()
        {
            CurrentMove.Apply(this, CurrentTarget);
        }

        public void AddCombatMove(CombatMove move)
        {
            CombatMoves.Add(move);
        }

        public string Team { get; set; }
        public List<CombatMove> CombatMoves { get; set; }
        public Action<Combattant, IEnumerable<Combattant>, List<CombatMove>> MovePicker { get; set; }
        public List<IApplyEffects> CombatEffects { get; set; } = new List<IApplyEffects>();
        public int CurrentRound { get; set; }

        public int ApplyDamage(int damage)
        {
            int actualDamage = damage - Armor;
            actualDamage = actualDamage < 0 ? 0 : actualDamage;
            CurrentHealth -= actualDamage;
            if (CurrentHealth < 1)
            {
                Die();
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
        public void PickMove(IEnumerable<Combattant> otherTeam)
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
        public int Initiative { get; set; }
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
}