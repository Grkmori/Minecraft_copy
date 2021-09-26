using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Parameters {
    public sealed class Region2D {
        /* Variables - For external use */
        public float regionSize { get; private set; }

        /* Variables - For internal use */
        private float subRegionWidth;
        private Vector2 minPosition;
        private Vector2 maxPosition;

        // Create a Region of the World to get Vector3/positions foreach "Quad"
        public Region2D(Vector3 startSizeCoords, Vector2 worldSizeChunks, float chunkWidth, float voxelWidth) {
            regionSize = (worldSizeChunks.x * worldSizeChunks.y) * // Size in Chunks
                         ((chunkWidth / voxelWidth) * (chunkWidth / voxelWidth)); // Quads per Chunk
            subRegionWidth = voxelWidth;
            minPosition = new Vector2((startSizeCoords.x - ((worldSizeChunks.x / 2) * chunkWidth)), (startSizeCoords.z - ((worldSizeChunks.x / 2) * chunkWidth)));
            maxPosition = new Vector2((startSizeCoords.x + ((worldSizeChunks.x / 2) * chunkWidth)), (startSizeCoords.x + ((worldSizeChunks.x / 2) * chunkWidth)));
        }

        // Create a Region of the World to get Vector3/positions foreach "Chunk"
        public Region2D(Vector3 startSizeCoords, Vector2 worldSizeChunks, float voxelWidth) {
            regionSize = worldSizeChunks.x * worldSizeChunks.y;
            subRegionWidth = 1;
            minPosition = new Vector2(startSizeCoords.x - (worldSizeChunks.x / 2), startSizeCoords.z - (worldSizeChunks.y / 2));
            maxPosition = new Vector2(startSizeCoords.x + (worldSizeChunks.x / 2), (startSizeCoords.x + (worldSizeChunks.y / 2)));
        }

        // Create a Region of the Chunk Vector3/positions foreach "Quad"
        public Region2D(Vector3 startSizeCoords, float chunkWidth, float voxelWidth) {
            regionSize = (chunkWidth / voxelWidth) * (chunkWidth / voxelWidth);
            subRegionWidth = voxelWidth;
            minPosition = new Vector2(startSizeCoords.x - (chunkWidth / 2), startSizeCoords.z - (chunkWidth / 2));
            maxPosition = new Vector2(startSizeCoords.x + (chunkWidth / 2), startSizeCoords.z + (chunkWidth / 2));
        }

        // IEnumerator required for "foreach(Vector3 vector3 in region)"
        public IEnumerator<Vector2> GetEnumerator() {
            for(float x = this.minPosition.x; x < maxPosition.x; x += subRegionWidth) {
                for(float y = this.minPosition.y; y < maxPosition.y; y += subRegionWidth) {
                    yield return new Vector2(x, y);
                }
            }
        }
    }
}