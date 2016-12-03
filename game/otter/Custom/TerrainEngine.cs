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
        private readonly TerrainConfig _terrainConfig;

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

        public TerrainType GetTerrainTypeFromConfig(int elevation, int moisture)
        {
            return _terrainConfig.GetTerrainType(elevation, moisture);
        }
    }

    public class TerrainConfig
    {
        public ElevationConfig AddElevationConfig(int @from, int to)
        {
            var elevationConfig = new ElevationConfig(@from, to, new List<MoistureConfig>());
            Config.Add(elevationConfig);
            return elevationConfig;
        }
        public List<ElevationConfig> Config { get; set; } = new List<ElevationConfig>();

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

            var elevationConfig = Config.Single(config => config.From < elevation && elevation <= config.To);
            var moistureConfig = elevationConfig.MoistureConfigs.Single(config => config.From < moisture && moisture <= config.To);
            return moistureConfig.TerrainType;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Config, TerrainConfigBuilder.GetSerializerSettings());
        }
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
        }

        public static TerrainConfig BuildTerrainConfig(string terrainData)
        {
            var terrainConfig = new TerrainConfig();
            var config =
                JsonConvert.DeserializeObject<List<ElevationConfig>>(
                    terrainData);
            terrainConfig.Config = config;
            return terrainConfig;
        }
        
        public static TerrainConfig BuildTerrainConfig()
        {
            var terrainConfig = new TerrainConfig();

            terrainConfig.AddElevationConfig(1, 17)
                .AddMoistureConfig(0, 100, TerrainType.Ocean);

            terrainConfig.AddElevationConfig(17, 20)
                .AddMoistureConfig(0, 100, TerrainType.Beach);

            terrainConfig.AddElevationConfig(20, 40)
                .AddMoistureConfig(0, 16, TerrainType.SubTropicalDesert)
                .AddMoistureConfig(16, 33, TerrainType.GrassLand)
                .AddMoistureConfig(33, 66, TerrainType.TropicalSeasonalForest)
                .AddMoistureConfig(66, 100, TerrainType.TropicalRainForest);

            terrainConfig.AddElevationConfig(40, 75)
                .AddMoistureConfig(0, 16, TerrainType.TemperateDesert)
                .AddMoistureConfig(16, 50, TerrainType.GrassLand)
                .AddMoistureConfig(50, 83, TerrainType.TemperateForest)
                .AddMoistureConfig(83,100, TerrainType.TemperateRainForest);
            
            terrainConfig.AddElevationConfig(75, 90)
                .AddMoistureConfig(0, 33, TerrainType.TemperateDesert)
                .AddMoistureConfig(33, 66, TerrainType.ShrubLand)
                .AddMoistureConfig(66, 100, TerrainType.Taiga);

            terrainConfig.AddElevationConfig(90, 100)
                .AddMoistureConfig(0, 10, TerrainType.Scorched)
                .AddMoistureConfig(10, 20, TerrainType.Tundra)
                .AddMoistureConfig(20, 50, TerrainType.Bare)
                .AddMoistureConfig(50, 100, TerrainType.Snow);

            return terrainConfig;
        }
    }

    public class ElevationConfig
    {
        public ElevationConfig()
        {
            MoistureConfigs = new List<MoistureConfig>();   
        }

        public ElevationConfig(int @from, int to, List<MoistureConfig> moistureConfigs)
        {
            From = @from;
            To = to;
            MoistureConfigs = moistureConfigs;
        }

        public ElevationConfig AddMoistureConfig(int @from, int to, TerrainType terrainType)
        {
            MoistureConfigs.Add(new MoistureConfig(@from, to, terrainType));
            return this;
        }

        public int From { get; set; }
        public int To { get; set; }
        public List<MoistureConfig> MoistureConfigs { get; set; }
    }

    public class MoistureConfig
    {
        public MoistureConfig()
        {
            
        }
        public MoistureConfig(int @from, int to, TerrainType terrainType)
        {
            From = @from;
            To = to;
            TerrainType = terrainType;
        }
        public int From { get; set; }
        public int To { get; set; }
        public TerrainType TerrainType { get; set; }
    }
}
