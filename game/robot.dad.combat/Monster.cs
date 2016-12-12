using System;
using System.Collections.Generic;

namespace robot.dad.combat
{
    public class Monster : Combattant
    {
        public Monster(string name, int health, int attackSkill, int defenseSkill, int armor, int initiative, string team, List<CombatMove> combatMoves, Action<Combattant, IEnumerable<Combattant>, List<CombatMove>> movePicker) 
            : base(name, health, attackSkill, defenseSkill, armor, initiative, team, combatMoves, movePicker)
        {
        }
    }
}