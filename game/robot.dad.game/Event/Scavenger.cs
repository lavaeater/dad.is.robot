using System.Collections.Generic;
using rds;
using robot.dad.common;

namespace robot.dad.game.Event
{
    /// <summary>
    /// SHould be two different things, creatable...character...not necessary?
    /// </summary>
    public class Scavenger : CreatableCharacter
    {
        public int Level { get; set; }

        public Scavenger(int level)
        {
            Name = CombatDemo.GetScavengerName();
            Level = level;
            Attack = 40 + level*10;
            Defense = 10 + level*10;
            Strength = 10 + level*10;
            MaxHealth = 30 + level*5;
            Armor = 5 + level*5;
            var weapons = new List<IWeapon>
            {
                new CharacterWeapon(CombatDemo.GetScavengerMoveName(), "En pangare", 5, true, 2, 35, 10, "skjuter")
            };
            ActiveWeapons = weapons;
        }

        public override IThing CreateInstance()
        {
            return new Scavenger(Level);
        }
    }
}