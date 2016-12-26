using System;
using System.Collections.Generic;

namespace robot.dad.common
{
    public interface ICombattant
    {
        string Name { get; }
        int CurrentAttack { get; }
        int CurrentDefense { get;}
        int CurrentArmor { get; }
        int CurrentHealth { get; set; }
        int CurrentMaxHealth { get; }
        int CurrentInitiative { get;}

        int CurrentRound { get; }
        bool Dead { get; }
        bool HasPicked { get; }
        string Team { get;  }

        CombatStatus Status { get;  }

        List<IApplyEffects> CurrentCombatEffects { get; }
        List<ICombatMove> CombatMoves { get; }

        ICombatMove CurrentMove { get; set; }
        ICombattant CurrentTarget { get; set; }
        IPickMove MovePicker { get; set; }
        Action<ICombattant> JustDied { get; set; }
        Action<ICombattant, int> TookDamage { get; set; }
        Action<ICombattant> RanAway { get; set; }

        void AddCombatMove(ICombatMove move);
        int ApplyDamage(int damage);
        void ClearMove();
        void Die();
        //TODO: Remove picked action, the method returns immediately
        //Rethink picking at a later time...
        void PickMove(IEnumerable<ICombattant> possibleTargets, Action picked);
        bool ResolveMove();
        void Runaway();
        string ToString();
    }
}