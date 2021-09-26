using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Parameters {
    public sealed class Region3D {
        /* Variables - For external use */
        public float regionSize { get; private set; }

        /* Variables - For internal use */
        private float subRegionWidth;
        private float subRegionHeight;
        private Vector3 minPosition;
        private Vector3 maxPosition;

        // Create a Region of the World to get Vector3/positions foreach "Voxel"
        public Region3D(Vector3 startSizeCoords, Vector2 worldSizeChunks, Vector2 chunkSize, Vector2 voxelSize) {
            regionSize = ((chunkSize.x / voxelSize.x) * ((chunkSize.y - startSizeCoords.y) / voxelSize.y) * (chunkSize.x / voxelSize.x)) * // Voxels per Chunk
                         (worldSizeChunks.x * worldSizeChunks.y); // Number of Chunks
            subRegionWidth = voxelSize.x;
            subRegionHeight = voxelSize.y;
            minPosition = new Vector3(startSizeCoords.x - ((worldSizeChunks.x / 2) * chunkSize.x), startSizeCoords.y, startSizeCoords.z - ((worldSizeChunks.x / 2) * chunkSize.x));
            maxPosition = new Vector3(startSizeCoords.x + ((worldSizeChunks.x / 2) * chunkSize.x), startSizeCoords.y + chunkSize.y, startSizeCoords.z + ((worldSizeChunks.x / 2) * chunkSize.x));
        }

        // Create a Region of the Chunk Vector3/positions foreach "Voxel"
        public Region3D(Vector3 startSizeCoords, Vector2 chunkSize, Vector2 voxelSize) {
            regionSize = (chunkSize.x / voxelSize.x) * (chunkSize.y / voxelSize.y) * (chunkSize.x / voxelSize.x);
            subRegionWidth = voxelSize.x;
            subRegionHeight = voxelSize.y;
            minPosition = new Vector3(startSizeCoords.x - (chunkSize.x / 2), startSizeCoords.y, startSizeCoords.z - (chunkSize.x / 2));
            maxPosition = new Vector3(startSizeCoords.x + (chunkSize.x / 2), startSizeCoords.y + chunkSize.y, startSizeCoords.z + (chunkSize.x / 2));
        }

        // IEnumerator required for "foreach(Vector3 vector3 in region)"
        public IEnumerator<Vector3> GetEnumerator() {
            for(float x = this.minPosition.x; x < maxPosition.x; x += subRegionWidth) {
                for(float y = this.minPosition.y; y < maxPosition.y; y += subRegionHeight) {
                    for(float z = this.minPosition.z; z < maxPosition.z; z += subRegionWidth) {
                        yield return new Vector3(x, y, z);
                    }
                }
            }
        }
    }
}