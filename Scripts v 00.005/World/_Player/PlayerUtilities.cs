using UnityEngine;
using BlueBird.World.Parameters;
///using BlueBird.World.WorldMap.Topography.Chunks;
using BlueBird.World.WorldMap.Topography.Voxels;

namespace BlueBird.World.Player {
    public sealed class PlayerUtilities {
        /* Instances */
        ///ChunkUtilities _chunkUtilities = new ChunkUtilities();
        VoxelUtilities _voxelUtilities = new VoxelUtilities();

        /* Variables - For Inputs */
        private static Vector3 mousePosition;
        private Vector3 rayDirectionNormalized;
        private Vector3 mousePositionCorrection = new Vector3(0.02f, Constants_str.voxelSize.y - 0.02f, 0.02f);
        private static Vector3 mousePositionNormalized;
        public static Vector3 mouseVoxelPosition;
        ///private static Vector3 mouseChunkPosition;

        public Vector3 GetMousePosition(LayerMask layerMask) {
            Ray mousePositionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousePositionHit;
            if(Physics.Raycast(mousePositionRay, out mousePositionHit, 250, layerMask)) {
                mousePosition = mousePositionHit.point;
                rayDirectionNormalized = new Vector3(mousePositionRay.direction.x / Mathf.Abs(mousePositionRay.direction.x),
                                                     mousePositionRay.direction.y / Mathf.Abs(mousePositionRay.direction.y),
                                                     mousePositionRay.direction.z / Mathf.Abs(mousePositionRay.direction.z));
                mousePositionNormalized = mousePosition + new Vector3(mousePositionCorrection.x * rayDirectionNormalized.x,
                                                                      mousePositionCorrection.y * -rayDirectionNormalized.y,
                                                                      mousePositionCorrection.z * rayDirectionNormalized.z);

                mouseVoxelPosition = _voxelUtilities.GetVoxelPositionFromPosition(mousePositionNormalized);
                ///mouseChunkPosition = _chunkUtilities.GetChunkPositionFromPosition(mousePositionNormalized);

                ///Debug.Log(mousePosition + " " + mouseVoxelPosition + " " + mouseChunkPosition);
            }

            return mouseVoxelPosition;
        }
    }
}