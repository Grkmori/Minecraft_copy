using UnityEngine;
using System.Collections.Generic;

namespace BlueBird.World.Parameters {
    public sealed class Region3D {
        /* Variables - For external use */
        public Vector3 minVector3 { get; private set; }
        public Vector3 maxVector3 { get; private set; }

        /* Variables - For internal use */
        private int subRegionWidth;
        private Vector3 minPosition;
        private Vector3 maxPosition;

        // Create a Region of the Chunk to get Vector3/positions foreach "Voxel"
        public Region3D(Vector3 @startSizeCoords, int @widthX, int @widthZ, int @quadWidth) {
            minVector3 = new Vector3(@startSizeCoords.x - .5f, @startSizeCoords.y, @startSizeCoords.z - .5f);
            maxVector3 = new Vector3(@widthX + .5f, @startSizeCoords.y, @widthZ + .5f);
            subRegionWidth = @quadWidth;
            minPosition = new Vector3(@startSizeCoords.x, @startSizeCoords.y, @startSizeCoords.z);
            maxPosition = new Vector3(@widthX, @startSizeCoords.y, @widthZ);
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