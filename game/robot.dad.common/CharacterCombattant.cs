using System;
using System.Collections.Generic;
using System.Linq;

namespace robot.dad.common
{
    public class CreatableCharacterCombattant : CharacterCombattant
    {
        public CreatableCharacterCombattant(ICharacter character, List<ICombatMove> combatMoves) : base(character)
        {
            CombatMoves = combatMoves;
        }

        public override List<ICombatMove> CombatMoves { get; }
    }

    public class CharacterCombattant : ICombattant
    {
        public ICharacter Character { get; set; }

        public CharacterCombattant(ICharacter character)
        {
            Character = character;
            CurrentHealth = Character.CurrentMaxHealth;
        }

        public int CurrentAttack => Character.CurrentAttack;
        public int CurrentDefense => Character.CurrentDefense;
        public int CurrentArmor => Character.CurrentDefense;
        public int CurrentHealth { get; set; }
        public int CurrentMaxHealth => Character.CurrentMaxHealth;
        public int CurrentInitiative => Character.CurrentInitiative;

        public List<IApplyEffects> CurrentCombatEffects { get; } = new List<IApplyEffects>();

        public virtual List<ICombatMove> CombatMoves
        {
            get
            {
                return Character.ActiveWeapons.Select(aw => aw.CombatMove).ToList();
            }
        }

        public ICombatMove CurrentMove { get; set; }
        public int CurrentRound { get; set; }
        public ICombattant CurrentTarget { get; set; }
        public bool Dead => Status == CombatStatus.Dead;

        public bool HasPicked { get; set; }

        public IPickMove MovePicker { get; set; }
        public string Name => Character.Name;
        public CombatStatus Status { get; set; }
        public string Team { get; set; }

        public Action<ICombattant, int> TookDamage { get; set; }
        public Action<ICombattant> RanAway { get; set; }
        public Action<ICombattant> JustDied { get; set; }

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

        public void AddCombatMove(ICombatMove move)
        {
            //May not be implemented for this particular class... we'll see...
            throw new NotImplementedException();
        }

        public void ClearMove()
        {
            CurrentTarget = null;
            CurrentMove = null;
        }

        public void Die()
        {
            Status = CombatStatus.Dead;
            JustDied?.Invoke(this);
        }

        public void PickMove(IEnumerable<ICombattant> possibleTargets, Action picked)
        {
            HasPicked = true;
            MovePicker?.PickMove(this, possibleTargets);
        }

        public bool ResolveMove()
        {
            HasPicked = false;
            return CurrentMove.Resolve(this, CurrentTarget);
        }

        public void Runaway()
        {
            Status = CombatStatus.Fled;
            RanAway?.Invoke(this);
        }
    }
}
