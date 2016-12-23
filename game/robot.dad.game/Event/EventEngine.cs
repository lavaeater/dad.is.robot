using System;
using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using rds;
using robot.dad.game.Entities;
using robot.dad.game.GameSession;
using robot.dad.graphics;
using Simplex;

namespace robot.dad.game.Event
{
    public class EventEngine
    {
        private readonly Noise _eventNoise;
        private readonly float _scale;
        private readonly Random _rand;
        private int _maxVal;

        public EventEngine()
        {
            _eventNoise = new Noise(42);
            _scale = 0.01f;
            _rand = new Random(32);
            _maxVal = 0;
        }

        public TileEvent GetEventForTile(CubicHexCoord coord, TerrainInfo terrainType)
        {
            int diceRoll = _rand.Next(1, 1001);//
            //int diceRoll = (int)_eventNoise.CalcPixel3D(coord.x, coord.y, 0, _scale).ForceRange(100, 1);
            if (diceRoll > _maxVal)
                _maxVal = diceRoll;
            if (terrainType.TerrainType == TerrainType.TemperateDesert || terrainType.TerrainType == TerrainType.SubTropicalDesert || terrainType.TerrainType == TerrainType.Scorched)
            {
                if (990< diceRoll && diceRoll <= 1000)
                {
                    return new TileEvent("Ruin", coord); //More thinking required!
                }
            }

            return null;
        }
    }

    public class Lootables
    {
        //Basic table for ruin scavengers - they are not always there, the scavengers, in the ruins        
    }

    public class CreatableCharacter : CreatableLootObject, ICharacter
    {
        public CreatableCharacter(int currentStrength, int currentMaxHealth, int currentAttack, int currentDefense, int currentArmor, IEnumerable<ICharacterComponent> playerComponents, IEnumerable<ICharacterComponent> activeComponents)
        {
            CurrentStrength = currentStrength;
            CurrentMaxHealth = currentMaxHealth;
            CurrentAttack = currentAttack;
            CurrentDefense = currentDefense;
            CurrentArmor = currentArmor;
            PlayerComponents = playerComponents;
            ActiveComponents = activeComponents;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Strength { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Armor { get; set; }
        public int CurrentStrength { get; }
        public int CurrentMaxHealth { get; }
        public int CurrentAttack { get; }
        public int CurrentDefense { get; }
        public int CurrentArmor { get; }
        public Dictionary<IITem, int> Inventory { get; set; } = new Dictionary<IITem, int>();
        public IEnumerable<ICharacterComponent> PlayerComponents { get; }
        public IEnumerable<ICharacterComponent> ActiveComponents { get; }
    }
}