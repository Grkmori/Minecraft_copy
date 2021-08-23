using System.Collections.Generic;
using UnityEngine;

namespace BlueBird.World {
    public sealed class Region2D {
        /* Variables - For external use */
        public Vector3 minVector3 { get; private set; }
        public Vector3 maxVector3 { get; private set; }
        public float numberOfVector3 { get; private set; }

        /* Variables - For internal use */
        public Vector3 minPosition;
        public Vector3 maxPosition;

        // Create a Region of the World to get Vector3/positions for the "foreach" function
        public Region2D(Vector3 @startSizeCoords, Vector3 @endSizeCoords) {
            minVector3 = new Vector3(@startSizeCoords.x - .5f, @startSizeCoords.y, @startSizeCoords.z - .5f);
            maxVector3 = new Vector3(@endSizeCoords.x + .5f, @startSizeCoords.y, @endSizeCoords.z + .5f);
            numberOfVector3 = (@endSizeCoords.x - @startSizeCoords.x) * (@endSizeCoords.z - @startSizeCoords.z);
            minPosition = new Vector3(@startSizeCoords.x, @startSizeCoords.y, @startSizeCoords.z);
            maxPosition = new Vector3(@endSizeCoords.x, @startSizeCoords.y, @endSizeCoords.x / 2);
        }

        // Create a Region of the Chunk Vector3/positions for the "foreach" function
        public Region2D(Vector3 @startSizeCoords, int @regionWidth) {
            minVector3 = new Vector3(@startSizeCoords.x - .5f, @startSizeCoords.y, @startSizeCoords.z - .5f);
            maxVector3 = new Vector3(minVector3.x + @regionWidth, @startSizeCoords.y, minVector3.z + @regionWidth);
            numberOfVector3 = @regionWidth * @regionWidth;
            minPosition = new Vector3(@startSizeCoords.x, @startSizeCoords.y, @startSizeCoords.z);
            maxPosition = new Vector3(@startSizeCoords.x + @regionWidth, @startSizeCoords.y, @startSizeCoords.z + @regionWidth);
        }

        // IEnumerator required for "foreach(Vector3 @vector3 in @region)"
        public IEnumerator<Vector3> GetEnumerator() {
            for(float x = this.minPosition.x; x <= maxPosition.x; x++) {
                for(float z = this.minPosition.z; z <= maxPosition.z; z++) {
                    float y = minPosition.y;
                    yield return new Vector3(x, y, z);
                }
            }
        }
    }
}