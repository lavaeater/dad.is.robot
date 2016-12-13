using System;
using System.Collections.Generic;

namespace robot.dad.combat
{
    public class Monster : Combattant
    {
        public Monster(string name, int health, int attackSkill, int defenseSkill, int armor, int initiative, string team, List<CombatMove> combatMoves, Action<Combattant, IEnumerable<Combattant>, List<CombatMove>,Action> movePicker) 
            : base(name, health, attackSkill, defenseSkill, armor, initiative, team, combatMoves, movePicker)
        {
        }
    }

    public abstract class MovePickerBase : IPickMoves
    {
        public Action DonePicking { get; set; }

        protected MovePickerBase(Action donePicking)
        {
            DonePicking = donePicking;
        }

        public virtual void PickMove(Combattant attacker, IEnumerable<Combattant> possibleTargets)
        {
            DonePicking();
        }
    }

    public interface IPickMoves
    {
        void PickMove(Combattant attacker, IEnumerable<Combattant> possibleTargets);
    }
}