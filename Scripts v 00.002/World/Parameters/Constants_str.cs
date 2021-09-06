using UnityEngine;

namespace BlueBird.World.Parameters {
    public struct Constants_str {
        /* Constants - For World */
        public static readonly Vector3 worldBaseVector3 = new Vector3(0, 0, 0);
        public static readonly float viewDistanceChunk = 4;
        public static readonly float gravity = -9.8f;

        /* Constants - For Chunk */
        public static readonly Vector2 chunkSize = new Vector2(8, 32);

        /* Constants - For Voxel */
        public static readonly Vector2 voxelsTextureAtlasSizePixels = new Vector2(64, 64);
        public static readonly Vector2 faceVoxelTextureSizePixels = new Vector2(16, 16);
        public static readonly Vector2 voxelSize = new Vector2(1, 1);

        /* Constants - For NoiseMap */
        //public static readonly float baseNoise = 0.02f;
        //public static readonly int baseHeight = 0;
        //public static readonly float baseNoiseHeight = 4;
        //public static readonly int elevation = 4;
        //public static readonly float frequency = 0.1f;

        /* Constants - For Character Camera */
        public static readonly float characterCameraHeight = 1f;

        /* Constants - For Characters */
        public static readonly Vector2 characterBaseRadius = new Vector2(0.3f, 0.4f);
        public static readonly float characterBaseHeight = 0.1f;

        public static readonly float characterBaseWalkSpeed = 3f;
        public static readonly float characterBaseSprintSpeed = 6f;
        public static readonly float characterBaseJumpForce = 6f;

        /* Constants - For Collider */
        public static readonly float onCollisionBounceBack = 0.0f;
    }
}