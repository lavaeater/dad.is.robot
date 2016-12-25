using System;
using System.Collections.Generic;
using robot.dad.common;

namespace robot.dad.combat
{
    public class Monster : Combattant
    {
        public Monster(string name, int health, int attackSkill, int defenseSkill, int armor, int initiative, string team, List<ICombatMove> combatMoves, IPickMove movePicker) 
            : base(name, health, attackSkill, defenseSkill, armor, initiative, team, combatMoves, movePicker)
        {
        }
    }

    public abstract class MovePickerBase : IPickMove
    {
        public Action DonePicking { get; set; }

        protected MovePickerBase(Action donePicking)
        {
            DonePicking = donePicking;
        }

        public abstract void PickMove(ICombattant attacker, IEnumerable<ICombattant> possibleTargets);
    }
}