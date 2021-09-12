using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Data.Topography.Noises {
    public sealed class Noise {
        public float Noise2D(Vector2 @position, float @scale, float @offSet) {
            return Mathf.PerlinNoise((((@position.x + 0.1f) / Constants_str.chunkSize.x) * @scale) + @offSet, (((@position.y + 0.1f) / Constants_str.chunkSize.x) * @scale) + @offSet);
        }

        public bool Noise3D(Vector3 @position, float @scale, float @threshold, float @offSet) {

            float x = (@position.x + @offSet + 0.1f) * @scale;
            float y = (@position.y + @offSet + 0.1f) * @scale;
            float z = (@position.z + @offSet + 0.1f) * @scale;

            float AB = Mathf.PerlinNoise(x, y);
            float BC = Mathf.PerlinNoise(y, z);
            float AC = Mathf.PerlinNoise(x, z);
            float BA = Mathf.PerlinNoise(y, x);
            float CB = Mathf.PerlinNoise(z, y);
            float CA = Mathf.PerlinNoise(z, x);

            if((AB + BC + AC + BA + CB + CA) / 6f > @threshold) {
                return true;
            } else {
                return false;
            }
        }
    }
}