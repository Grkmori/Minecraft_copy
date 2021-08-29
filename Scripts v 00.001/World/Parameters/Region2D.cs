using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Parameters {
    public sealed class Region2D {
        /* Variables - For external use */
        public float regionSize { get; private set; }

        /* Variables - For internal use */
        private float subRegionWidth;
        private Vector3 minPosition;
        private Vector3 maxPosition;

        // Create a Region of the World to get Vector3/positions foreach "Quad/Voxel"
        public Region2D(Vector3 @startSizeCoords, Vector2 @worldSize, int @chunkWidth) {
            regionSize = (((@worldSize.x * @chunkWidth) - (@startSizeCoords.x - 1)) / Constants_str.voxelWidth) * (((@worldSize.y * @chunkWidth) - (@startSizeCoords.z - 1)) / Constants_str.voxelWidth);
            subRegionWidth = Constants_str.voxelWidth;
            minPosition = new Vector3(@startSizeCoords.x, @startSizeCoords.y, @startSizeCoords.z);
            maxPosition = new Vector3(@worldSize.x, @startSizeCoords.y, @worldSize.y);
        }

        // Create a Region of the World to get Vector3/positions foreach "Chunk"
        public Region2D(Vector3 @startSizeCoords, Vector2 @worldSize) {
            regionSize = ((@worldSize.x - (@startSizeCoords.x - 1)) / Constants_str.voxelWidth) * ((@worldSize.y - (@startSizeCoords.z - 1)) / Constants_str.voxelWidth);
            subRegionWidth = Constants_str.voxelWidth;
            minPosition = new Vector3(@startSizeCoords.x, @startSizeCoords.y, @startSizeCoords.z);
            maxPosition = new Vector3(@worldSize.x, @startSizeCoords.y, @worldSize.y);
        }

        // Create a Region of the Chunk Vector3/positions foreach "Quad/Voxel"
        public Region2D(Vector3 @startSizeCoords, int @regionWidth) {
            regionSize = ((@regionWidth / Constants_str.voxelWidth) * (@regionWidth / Constants_str.voxelWidth));
            subRegionWidth = Constants_str.voxelWidth;
            minPosition = new Vector3(@startSizeCoords.x, @startSizeCoords.y, @startSizeCoords.z);
            maxPosition = new Vector3(@startSizeCoords.x + @regionWidth, @startSizeCoords.y, @startSizeCoords.z + @regionWidth);
        }

        // IEnumerator required for "foreach(Vector3 @vector3 in @region)"
        public IEnumerator<Vector3> GetEnumerator() {
            for(float x = this.minPosition.x; x <= maxPosition.x; x = x + subRegionWidth) {
                for(float z = this.minPosition.z; z <= maxPosition.z; z = z + subRegionWidth) {
                    float y = minPosition.y;
                    yield return new Vector3(x, y, z);
                }
            }
        }
    }
}