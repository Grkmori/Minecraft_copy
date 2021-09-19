using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Definitions;

namespace BlueBird.World.Data.Topography.Noises {
    public sealed class NoiseGenerator {
        /* Instances */
        Noise _noise = new Noise();

        /* Variables - For Noises */
        private readonly int offSet = Settings_str.noiseOffSet;
        private int noiseTerrainHeight;

        public void InitializeSeed() {
            //Random.State oldState = Random.state; // Saves State before Seed Initialization, used to always get the same result for the seed
            Random.InitState(Settings_str.noiseSeed); // Initializes the Random Number Generator state with a Seed
            //Random.state = oldState; // Load State before Seed Initialization
        }

        public int TerrainNoise(Vector3 position, BiomeDefinition biome) {
            noiseTerrainHeight = Mathf.RoundToInt(biome.terrainHeight * _noise.Noise2D(
                                                 new Vector2(position.x, position.z), biome.terrainScale, offSet)) +
                                                 biome.solidGroundHeight;

            return noiseTerrainHeight;
        }

        public bool Noise3D(Vector3 position, float scale, float threshold) {
            return _noise.Noise3D(position, scale, threshold, offSet);
        }
    }
}
