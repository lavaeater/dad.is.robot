using System;
using System.Collections.Generic;
using System.Text;
using robot.dad.combat.Interfaces;

namespace robot.dad.combat
{
    public class Combattant : ICombattant
    {
        public Combattant(string name, int health, int attackSkill, int defenseSkill, int armor, int initiative, string team, List<ICombatMove> combatMoves, IPickMove movePicker)
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

        public bool ResolveMove()
        {
            return CurrentMove.Apply(this, CurrentTarget);
        }

        public void AddCombatMove(ICombatMove move)
        {
            CombatMoves.Add(move);
        }

        public string Team { get; set; }
        public List<ICombatMove> CombatMoves { get; set; }
        public IPickMove MovePicker { get; set; }
        public List<IApplyEffects> CombatEffects { get; set; } = new List<IApplyEffects>();
        public int CurrentRound { get; set; }
        public Action<ICombattant, int> TookDamage { get; set; }
        public int ApplyDamage(int damage)
        {
            int actualDamage = damage - Armor;
            actualDamage = actualDamage < 0 ? 0 : actualDamage;
            CurrentHealth -= actualDamage;
            TookDamage?.Invoke(this, actualDamage);
            if (CurrentHealth < 1)
            {
                Die();
            }
            return actualDamage;
        }

        public CombatStatus Status { get; set; }

        public void Die()
        {
            IJustDied?.Invoke(this);
            Status = CombatStatus.Dead;
        }

        public void Runaway()
        {
            Status = CombatStatus.Fled;
        }

        public ICombatMove CurrentMove { get; set; }
        //Choose a target as well!
        public void PickMove(IEnumerable<ICombattant> possibleTargets, Action picked)
        {
            MovePicker?.PickMove(this, possibleTargets);
        }

        public ICombattant CurrentTarget { get; set; }
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
        public Action<ICombattant> IJustDied { get; set; }

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