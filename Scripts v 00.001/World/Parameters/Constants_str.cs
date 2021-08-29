using UnityEngine;

namespace BlueBird.World.Parameters {
    public struct Constants_str {
        /* Constants - World */
        public static readonly Vector3Int worldBaseVector3 = new Vector3Int(1, 1, 1);

        /* Constants - Chunk */
        public static readonly int chunkWidth = 16;
        public static readonly int chunkHeight = 6;

        /* Constants - Voxel */
        public static readonly float voxelTextureWidthPixels = 16;
        public static readonly float voxelTextureHeightPixels = 16;
        public static readonly float voxelWidth = 1;
        public static readonly float voxelHeight = 1;

        /* Constants - For NoiseMap */
        public static readonly float baseNoise = 0.02f;
        public static readonly int baseHeight = 0;
        public static readonly float baseNoiseHeight = 4;
        public static readonly int elevation = 4;
        public static readonly float frequency = 0.1f;
    }
}