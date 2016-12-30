using System;
using System.Collections.Generic;
using System.Text;
using robot.dad.common;

namespace robot.dad.combat
{
    public class Combattant : ICombattant
    {
        public Combattant(string name, int currentMaxHealth, int currentAttack, int currentDefense, int currentArmor, int currentInitiative, string team, List<ICombatMove> combatMoves, IPickMove movePicker)
        {
            Name = name;
            CurrentMaxHealth = currentMaxHealth;
            CurrentHealth = CurrentMaxHealth;
            CurrentAttack = currentAttack;
            CurrentDefense = currentDefense;
            CurrentArmor = currentArmor;
            CurrentInitiative = currentInitiative;
            Team = team;
            CombatMoves = combatMoves;
            MovePicker = movePicker;
            Status = CombatStatus.Active;
        }

        public bool ResolveMove()
        {
            return CurrentMove.Resolve(this, CurrentTarget);
        }

        public Action<ICombattant> RanAway { get; set; }

        public void AddCombatMove(ICombatMove move)
        {
            CombatMoves.Add(move);
        }

        public string Team { get; set; }
        public List<ICombatMove> CombatMoves { get; set; }
        public IPickMove MovePicker { get; set; }
        public List<IApplyEffects> CurrentCombatEffects { get; set; } = new List<IApplyEffects>();
        public int CurrentRound { get; set; }
        public Action<ICombattant, int> TookDamage { get; set; }
        public int ApplyDamage(int damage)
        {
            int actualDamage = damage - CurrentArmor;
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
            Status = CombatStatus.Dead;
            JustDied?.Invoke(this);
        }

        public void Runaway()
        {
            Status = CombatStatus.Fled;
            RanAway?.Invoke(this);
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
        public int CurrentMaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int CurrentDefense { get; set; }
        public int CurrentArmor { get; set; }
        public int CurrentInitiative { get; set; }
        public int CurrentAttack { get; set; }

        public bool Npc { get; set; }
        public string Name { get; set; }
        public Action<ICombattant> JustDied { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Name}");
            sb.AppendLine($"Hälsa: {CurrentHealth} / {CurrentMaxHealth}");
            sb.AppendLine($"Status: {Status}");
            return sb.ToString();
        }
    }
}