using UnityEngine;

namespace BlueBird.World.Parameters {
    public struct Constants_str {
        /* Constants - For World */
        public static readonly Vector3 worldBaseVector3 = new Vector3(0, 0, 0);
        public static readonly float viewDistanceChunk = 4;
        //public static readonly float gravity = -9.8f;

        /* Constants - For Chunk */
        public static readonly Vector2 chunkSize = new Vector2(8, 32);

        /* Constants - For Voxel */
        public static readonly Vector2 voxelsTextureAtlasSizePixels = new Vector2(64, 64);
        public static readonly Vector2 faceVoxelTextureSizePixels = new Vector2(16, 16);
        public static readonly Vector2 voxelSize = new Vector2(1, 1);
        public static readonly string defaultVoxelTypeName = "Air";

        /* Constants - For NoiseMap */
        //public static readonly float baseNoise = 0.02f;
        //public static readonly int baseHeight = 0;
        //public static readonly float baseNoiseHeight = 4;
        //public static readonly int elevation = 4;
        //public static readonly float frequency = 0.1f;

        /* Constants - For GameTime */
        public static readonly float gameNormalSpeed = 1;
        public static readonly float gameFastSpeed = 2;
        public static readonly float gameSuperFastSpeed = 4;
        public static readonly float gameTicksPerSecond = 8;

        public static readonly float defaultDelayTimeInSeconds = 0.25f;

        /* Settings - For Player Main Camera */
        public static Vector3 mainCameraFocusPosition = new Vector3(0, 20, 0);
        public static Vector3 mainCameraFocusRotation = new Vector3(0, 30, 0);
        public static Vector3 mainCameraPosition = new Vector3(0, 15, -30);
        public static Vector3 mainCameraRotation = new Vector3(30, 0, 0);
        public static Vector3 mainCameraScale = Vector3.one;
        public static float mainCameraFieldOfView = 30;

        /* Constants - For Player UI */
        public static readonly float toolbarHeightInCanvas = 10;
        public static readonly int toolbarItemSlotsNumber = 9;

        /* Constants - For Characters */
        public static readonly Vector3 characterBaseRadius = new Vector3(0.3f, 0.4f, 0.3f);
        public static readonly Vector3 charcterColliderSize = new Vector3(1f, 1f, 1f);

        public static readonly float characterBaseWalkSpeed = 3;
        public static readonly float characterBaseSprintSpeed = 6;
        public static readonly float characterMaximumVelocityChangeForce = 1;
        public static readonly float characterBaseJumpForce = 260;
        public static readonly float characterJumpDelay = 0.6f;

        /* Constants - For Character Camera */
        public static readonly Vector3 characterCameraPosition = new Vector3(0, 1f, 0);
        public static readonly float characterCameraFieldOfView = 60;
        public static readonly float characterCameraNearClipPlane = 0.01f;

        public static readonly float cursorCheckIncrement = 0.1f;
        public static readonly float minCursorReach = 1f;
        public static readonly float maxCursorReach = 3;

        /* Constants - For Collider */
        public static readonly float boxColliderHeightForGroundCheck = 0.1f;
        public static readonly Vector3 boxColliderSizeOffSetForGroundCheck = new Vector3(-0.05f, 0, -0.05f);
    }
}