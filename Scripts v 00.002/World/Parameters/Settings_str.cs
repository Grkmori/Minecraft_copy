using UnityEngine;

namespace BlueBird.World.Parameters {
    public struct Settings_str {
        /* Settings - For Threading */
        public static bool enableThreading = true;
        public static readonly int maxThreads = 8; // TODO: Use SystemInfo.processorCount ??

        /* Settings - For World */
        public static Vector2 worldSizeChunks = new Vector2(16, 16);

        /* Settings - For NoiseMap */
        public static int noiseSeed = 1;
        public static int noiseOffSet = 0;

        /* Settings - For Player */
        public static Vector3 playerSpawnPosition = new Vector3(Constants_str.worldBaseVector3.x, Constants_str.chunkSize.y - 2, Constants_str.worldBaseVector3.z);
        public static float mouseSensitivity = 500f;
    }
}