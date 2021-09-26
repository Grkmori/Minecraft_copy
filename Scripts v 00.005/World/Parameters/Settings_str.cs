using UnityEngine;

namespace BlueBird.World.Parameters {
    public struct Settings_str {
        /* Settings - For Game Video Settings */
        public static int vSync = 1; // For a 60Hz screen 1 = 60fps, 2 = 30fps, 0 = don't wait for sync.
        public static int targetFPS = 60;

        /* Settings - For Threading */
        public static bool enableThreading = true;
        public static readonly int maxThreads = 16; // TODO: Use SystemInfo.processorCount ??

        /* Settings - For World */
        public static Vector2 worldSizeChunks = new Vector2(8, 8);

        /* Settings - For NoiseMap */
        public static int noiseSeed = 1;
        public static int noiseOffSet = 0;

        /* Settings - For Player */
        public static Vector3 playerSpawnPosition = new Vector3(Constants_str.worldBaseVector3.x, Constants_str.chunkSize.y, Constants_str.worldBaseVector3.z);
        public static float mouseSensitivity = 200;

        /* Settings - For Main Camera */
        public static float mainCameraMovementTimeSpeed = 1;
        public static float mainCameraNormalMovementSpeed = 10;
        public static float mainCameraFastMovementSpeed = mainCameraNormalMovementSpeed * 2;
        public static float mainCameraMouseMovementSensitivity = 1;
        public static float mainCameraRotationSpeed = 0.5f;
        public static Vector3 mainCameraZoomAmount = new Vector3(0, -0.5f, 1);
        public static Vector3 mainCameraMinimumZoomAmount = new Vector3(0, -10, -20);
        public static Vector3 mainCameraMaximumZoomAmount = new Vector3(0, +10, +20);
    }
}