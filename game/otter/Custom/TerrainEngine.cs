using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Simplex;

namespace Otter.Custom
{
    public class TerrainEngine
    {
        private readonly Noise _terrainNoise;
        private readonly Noise _moistureNoise;
        private readonly float _terrainScale;
        private readonly float _moistureScale;
        private TerrainConfig _terrainConfig;

        public TerrainEngine(int seed, float terrainScale, float moistureScale, string terrainData)
        {
            _terrainScale = terrainScale;
            _moistureScale = moistureScale;
            _terrainNoise = new Noise(seed);
            _moistureNoise = new Noise();
            _terrainConfig = TerrainConfigBuilder.BuildTerrainConfig(terrainData);
        }

        public TerrainInfo GetTerrainTypeForCoord(int x, int y)
        {
            int elevation = (int)_terrainNoise.CalcPixel3D(x, y, 0, _terrainScale).ForceRange(100, 1);
            int moisture = (int)_moistureNoise.CalcPixel3D(x, y, 0, _moistureScale).ForceRange(100, 1);
            TerrainType terrainType = GetTerrainTypeFromConfig(elevation, moisture);
            return new TerrainInfo(terrainType, elevation, moisture);
        }

        public TerrainType GetTerrainType(int elevation, int moisture)
        {
            TerrainType terrain = TerrainType.Ocean;
            if (17 < elevation && elevation <= 20)
            {
                terrain = TerrainType.Beach;
            }
            if (20 < elevation && elevation <= 40)
            {
                terrain = TerrainType.TropicalRainForest;
                if (0 < moisture && moisture <= 16)
                    terrain = TerrainType.SubTropicalDesert;
                if (16 < moisture && moisture <= 33)
                    terrain = TerrainType.GrassLand;
                if (33 < moisture && moisture <= 66)
                    terrain = TerrainType.TropicalSeasonalForest;
            }
            if (40 < elevation && elevation <= 75)
            {
                terrain = TerrainType.TemperateRainForest;
                if (0 < moisture && moisture <= 16)
                    terrain = TerrainType.TemperateDesert;
                if (16 < moisture && moisture <= 50)
                    terrain = TerrainType.GrassLand;
                if (50 < moisture && moisture <= 83)
                    terrain = TerrainType.TemperateForest;
            }
            if (75 < elevation && elevation <= 90)
            {
                terrain = TerrainType.Taiga;
                if (0 < moisture && moisture <= 33)
                    terrain = TerrainType.TemperateDesert;
                if (33 < moisture && moisture <= 66)
                    terrain = TerrainType.ShrubLand;
            }
            if (90 < elevation && elevation <= 100)
            {
                terrain = TerrainType.Snow;
                if (0 < moisture && moisture <= 10)
                    terrain = TerrainType.Scorched;
                if (10 < moisture && moisture <= 20)
                    terrain = TerrainType.Bare;
                if (20 < moisture && moisture <= 50)
                    terrain = TerrainType.Tundra;
            }
            return terrain;
        }

        public TerrainType GetTerrainTypeFromConfig(int elevation, int moisture)
        {
            _terrainConfig = TerrainConfigBuilder.BuildTerrainConfig();
            return _terrainConfig.GetTerrainType(elevation, moisture);
        }

        private static TerrainType Terrain(int elevation, int moisture, TerrainConfig terrainConfig)
        {
            return terrainConfig.Config.Single(
                elevConfig => elevConfig.Key.Item1 < elevation && elevation <= elevConfig.Key.Item2)
                .Value.Single(moistConfig => moistConfig.Key.Item1 < moisture && moisture <= moistConfig.Key.Item2)
                .Value;
        }
    }

    

    public class TerrainConfig
    {
        public Dictionary<RangeInt, Dictionary<RangeInt, TerrainType>> Config2 = new Dictionary<RangeInt, Dictionary<RangeInt, TerrainType>>();

        public Dictionary<Tuple<int, int>, Dictionary<Tuple<int, int>, TerrainType>> Config =
            new Dictionary<Tuple<int, int>, Dictionary<Tuple<int, int>, TerrainType>>();

        public TerrainType GetTerrainType(int elevation, int moisture)
        {
            if (elevation > 100)
                elevation = 100;
            if (elevation < 1)
                elevation = 1;

            if (moisture > 100)
                moisture = 100;
            if (moisture < 1)
                moisture = 1;
            return Config.Single(
                elevConfig => elevConfig.Key.Item1 < elevation && elevation <= elevConfig.Key.Item2)
                .Value.Single(moistConfig => moistConfig.Key.Item1 < moisture && moisture <= moistConfig.Key.Item2)
                .Value;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Config2, TerrainConfigBuilder.GetSerializerSettings());
        }
    }

    public class RangeInt
    {
        public RangeInt()
        {

        }
        public RangeInt(int min, int max)
        {
            Min = min;
            Max = max;
        }
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class TerrainConfigBuilder
    {
        public static JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = false
            });
            return settings;
            ;
        }
        public static TerrainConfig BuildTerrainConfig(string terrainData)
        {
            var terrainConfig = new TerrainConfig();
            var config =
                JsonConvert.DeserializeObject<Dictionary<RangeInt, Dictionary<RangeInt, TerrainType>>>(
                    terrainData);
            terrainConfig.Config2 = config;
            return terrainConfig;
        }

        public static TerrainConfig BuildTerrainConfig2()
        {
            var terrainConfig = new TerrainConfig();

            var oceanConfig = new Dictionary<RangeInt, TerrainType>
            {
                {new RangeInt(0, 100), TerrainType.Ocean}
            };

            terrainConfig.Config2.Add(new RangeInt(0, 17), oceanConfig);

            var beachConfig = new Dictionary<RangeInt, TerrainType>
            {
                {new RangeInt(0, 100), TerrainType.Beach}
            };

            terrainConfig.Config2.Add(new RangeInt(17, 20), beachConfig);

            var config = new Dictionary<RangeInt, TerrainType>
            {
                {new RangeInt(66, 100), TerrainType.TropicalRainForest},
                {new RangeInt(0, 16), TerrainType.SubTropicalDesert},
                {new RangeInt(16, 33), TerrainType.GrassLand},
                {new RangeInt(33, 66), TerrainType.TropicalSeasonalForest}
            };

            terrainConfig.Config2.Add(new RangeInt(20, 40), config);

            config = new Dictionary<RangeInt, TerrainType>
            {
                {new RangeInt(0, 16), TerrainType.TemperateDesert},
                {new RangeInt(16, 50), TerrainType.GrassLand},
                {new RangeInt(50, 83), TerrainType.TemperateForest},
                {new RangeInt(83, 100), TerrainType.TemperateRainForest}
            };

            terrainConfig.Config2.Add(new RangeInt(40, 75), config);
            config = new Dictionary<RangeInt, TerrainType>
            {
                {new RangeInt(0, 33), TerrainType.TemperateDesert},
                {new RangeInt(33, 66), TerrainType.ShrubLand},
                {new RangeInt(66, 100), TerrainType.Taiga}
            };

            terrainConfig.Config2.Add(new RangeInt(75, 90), config);

            config = new Dictionary<RangeInt, TerrainType>
            {
                {new RangeInt(0, 10), TerrainType.Scorched},
                {new RangeInt(10, 20), TerrainType.Bare},
                {new RangeInt(20, 50), TerrainType.Tundra},
                { new RangeInt(50, 100), TerrainType.Snow}
            };

            terrainConfig.Config2.Add(new RangeInt(90, 100), config);

            return terrainConfig;
        }

        public static TerrainConfig BuildTerrainConfig()
        {
            var terrainConfig = new TerrainConfig();

            var oceanConfig = new Dictionary<Tuple<int, int>, TerrainType>
            {
                {new Tuple<int, int>(0, 100), TerrainType.Ocean}
            };

            terrainConfig.Config.Add(new Tuple<int, int>(0, 17), oceanConfig);

            var beachConfig = new Dictionary<Tuple<int, int>, TerrainType>
            {
                {new Tuple<int, int>(0, 100), TerrainType.Beach}
            };

            terrainConfig.Config.Add(new Tuple<int, int>(17, 20), beachConfig);

            var config = new Dictionary<Tuple<int, int>, TerrainType>
            {
                {new Tuple<int, int>(66, 100), TerrainType.TropicalRainForest},
                {new Tuple<int, int>(0, 16), TerrainType.SubTropicalDesert},
                {new Tuple<int, int>(16, 33), TerrainType.GrassLand},
                {new Tuple<int, int>(33, 66), TerrainType.TropicalSeasonalForest}
            };

            terrainConfig.Config.Add(new Tuple<int, int>(20, 40), config);

            config = new Dictionary<Tuple<int, int>, TerrainType>
            {
                {new Tuple<int, int>(0, 16), TerrainType.TemperateDesert},
                {new Tuple<int, int>(16, 50), TerrainType.GrassLand},
                {new Tuple<int, int>(50, 83), TerrainType.TemperateForest},
                {new Tuple<int, int>(83, 100), TerrainType.TemperateRainForest}
            };

            terrainConfig.Config.Add(new Tuple<int, int>(40, 75), config);
            config = new Dictionary<Tuple<int, int>, TerrainType>
            {
                {new Tuple<int, int>(0, 33), TerrainType.TemperateDesert},
                {new Tuple<int, int>(33, 66), TerrainType.ShrubLand},
                {new Tuple<int, int>(66, 100), TerrainType.Taiga}
            };

            terrainConfig.Config.Add(new Tuple<int, int>(75, 90), config);

            config = new Dictionary<Tuple<int, int>, TerrainType>
            {
                {new Tuple<int, int>(0, 10), TerrainType.Scorched},
                {new Tuple<int, int>(10, 20), TerrainType.Bare},
                {new Tuple<int, int>(20, 50), TerrainType.Tundra},
                { new Tuple<int, int>(50, 100), TerrainType.Snow}
            };

            terrainConfig.Config.Add(new Tuple<int, int>(90, 100), config);

            return terrainConfig;
        }
    }
}
