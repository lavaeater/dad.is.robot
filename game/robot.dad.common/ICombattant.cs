using System;
using System.Collections.Generic;

namespace robot.dad.common
{
    public interface ICombattant
    {
        int Armor { get; set; }
        int AttackSkill { get; set; }
        List<IApplyEffects> CombatEffects { get; set; }
        List<ICombatMove> CombatMoves { get; set; }
        int CurrentHealth { get; set; }
        ICombatMove CurrentMove { get; set; }
        int CurrentRound { get; set; }
        ICombattant CurrentTarget { get; set; }
        bool Dead { get; }
        int DefenseSkill { get; set; }
        bool HasPicked { get; }
        int Health { get; set; }
        Action<ICombattant> IJustDied { get; set; }
        int Initiative { get; set; }
        IPickMove MovePicker { get; set; }
        string Name { get; set; }
        bool Npc { get; set; }
        CombatStatus Status { get; set; }
        string Team { get; set; }
        Action<ICombattant, int> TookDamage { get; set; }

        void AddCombatMove(ICombatMove move);
        int ApplyDamage(int damage);
        void ClearMove();
        void Die();
        void PickMove(IEnumerable<ICombattant> possibleTargets, Action picked);
        bool ResolveMove();
        void Runaway();
        string ToString();
    }
}