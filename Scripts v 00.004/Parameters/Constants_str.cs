using UnityEngine;

namespace BlueBird.World.Parameters {
    public struct Constants_str {
        /* Constants - For World */
        public static readonly Vector3 worldBaseVector3 = new Vector3(0, 0, 0);
        public const float viewDistanceChunk = 4;
        //public const float gravity = -9.8f;

        /* Constants - For Chunk */
        public static readonly Vector2 chunkSize = new Vector2(8, 32);

        /* Constants - For Voxel */
        public static readonly Vector2 voxelSize = new Vector2(1, 1);
        public const string defaultVoxelTypeName = "Air";
        public static readonly Vector3 baseVoxelTerrain = new Vector3(0, -voxelSize.y, 0);
        public static readonly Vector3 topVoxelAir = new Vector3(0, +voxelSize.y, 0);
        public static readonly Vector2 voxelsTextureAtlasSizePixels = new Vector2(64, 64);
        public static readonly Vector2 faceVoxelTextureSizePixels = new Vector2(16, 16);

        /* Constants - For NoiseMap */
        //public const float baseNoise = 0.02f;
        //public const float baseHeight = 0;
        //public const float baseNoiseHeight = 4;
        //public const float elevation = 4;
        //public const float frequency = 0.1f;

        /* Constants - For GameTime */
        public const float gameNormalSpeed = 1;
        public const float gameFastSpeed = 2;
        public const float gameSuperFastSpeed = 4;
        public const float gameTicksPerSecond = 8;

        public const float defaultDelayTimeInSeconds = 0.25f;

        /* Settings - For Player Main Camera */
        public static readonly Vector3 mainCameraFocusPosition = new Vector3(0, 20, 0);
        public static readonly Vector3 mainCameraFocusRotation = new Vector3(0, 30, 0);
        public static readonly Vector3 mainCameraPosition = new Vector3(0, 15, -30);
        public static readonly Vector3 mainCameraRotation = new Vector3(30, 0, 0);
        public static readonly Vector3 mainCameraScale = Vector3.one;
        public const float mainCameraFieldOfView = 30;

        /* Constants - For Player UI */
        public const float toolbarHeightInCanvas = 10;
        public const int toolbarItemSlotsNumber = 9;

        /* Constants - For Characters */
        public static readonly Vector3 characterBaseRadius = new Vector3(0.3f, 0.4f, 0.3f);
        public static readonly Vector3 charcterColliderSize = new Vector3(1f, 1f, 1f);

        public const float characterBaseWalkSpeed = 3;
        public const float characterSprintSpeedMultiplier = 1.5f;
        public const float characterMaximumVelocityChangeForce = 1;
        public const float characterBaseJumpForce = 260;
        public const float characterJumpDelay = 0.6f;

        /* Constants - For Character Camera */
        public static readonly Vector3 characterCameraPosition = new Vector3(0, 1f, 0);
        public const float characterCameraFieldOfView = 60;
        public const float characterCameraNearClipPlane = 0.01f;

        public const float cursorCheckIncrement = 0.1f;
        public const float minCursorReach = 1f;
        public const float maxCursorReach = 3;

        /* Constants - For Collider */
        public const float boxColliderHeightForGroundCheck = 0.05f;
        public static readonly Vector3 boxColliderSizeOffSetForGroundCheck = new Vector3(-0.05f, 0, -0.05f);

        /* Constants - For Pathfinding */
        public const float baseVoxelMovementCost = 10;
        public const float moveStraitCost = 10;
        public const float moveDiagonalCost = 14;
        public const float moveVerticalCost = 14;
    }
}