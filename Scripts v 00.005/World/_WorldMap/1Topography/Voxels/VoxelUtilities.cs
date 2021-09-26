using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.WorldMap.Topography.Chunks;

namespace BlueBird.World.WorldMap.Topography.Voxels {
    public sealed class VoxelUtilities {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        /* Variables - For Voxels */
        private static readonly float voxelWidth = Constants_str.voxelSize.x;
        private static readonly float voxelHeight = Constants_str.voxelSize.y;

        public Vector3 GetVoxelPositionFromPosition(Vector3 position) {
            float x = Mathf.RoundToInt(position.x);
            float y = Mathf.RoundToInt(position.y);
            float z = Mathf.RoundToInt(position.z);
            return new Vector3(x, y, z);
        }

        public bool CheckFacesOutOfChunk(Vector3 voxelFacePosition, Voxel voxelData, out Voxel cursorFaceVoxel) {
            Vector3 chunkDataPosition = voxelData._chunk.chunkPosition;
            Vector3 chunkFacePosition = _chunkUtilities.GetChunkPositionFromPosition(voxelFacePosition);

            if(WorldMapData.dictionaryChunkData.ContainsKey(chunkFacePosition)) {
                if(!WorldMapData.dictionaryChunkData[chunkDataPosition].dictionaryChunkVoxels.ContainsKey(voxelFacePosition)) {
                    cursorFaceVoxel = WorldMapData.dictionaryChunkData[chunkFacePosition].dictionaryChunkVoxels[voxelFacePosition];
                    return true;
                } else {
                    cursorFaceVoxel = voxelData; // Just to return some value to 'out' parameter, this wont be used (because of the 'if statement' on the calling function)
                    return false;
                }
            } else {
                cursorFaceVoxel = voxelData; // Just to return some value to 'out' parameter, this wont be used (because of the 'if statement' on the calling function)
                return false;
            }
        }

        public bool CheckVoxelFace(Vector3 facePosition, Vector3 chunkPosition, Chunk chunkData, Voxel voxelData) {
            // Setting up
            Vector3 voxelFacePosition = facePosition + chunkPosition;
            Vector3 chunkFacePosition = _chunkUtilities.GetChunkPositionFromPosition(voxelFacePosition);

            // Checking if Voxel "Faces"(Voxel next to the current Voxel) to be Rendered or Not
            if(WorldMapData.dictionaryChunkData.ContainsKey(chunkFacePosition)) {
                if(chunkData.dictionaryChunkVoxels.ContainsKey(voxelFacePosition)) {
                    ///Debug.Log("If <b>'VoxelFace' " + voxelFacePosition + "<b> is <color=orange><i>inside of 'Chunk Bounds'</i></color>, return <i><b>'VoxelFace'</b><color=blue> isTransparent</color></i>.");
                    return chunkData.dictionaryChunkVoxels[voxelFacePosition].isTransparent;
                } else {
                    if(voxelFacePosition.y < 0) {
                        ///Debug.Log("If <b>'VoxelFace' " + voxelFacePosition + "</b> is <color=orange><i>under(y < 0) 'Chunk Bounds'</i></color>, return <i><color=blue>False</color></i>.");
                        return false;
                    } else {
                        ///Debug.Log("If <b>'VoxelFace' " + voxelFacePosition + "</b> is <color=orange><i>out of 'Chunk Bounds'</i></color>, return if <i><b>'VoxelFace'</b><color=blue> isTransparent</color></i>.");
                        return WorldMapData.dictionaryChunkData[chunkFacePosition].dictionaryChunkVoxels[voxelFacePosition].isTransparent;
                    }
                }
            } else {
                ///Debug.Log("If <b>'VoxelFace' " + voxelFacePosition + "</b> is <color=orange><i>out of 'World Bounds'</i></color>, return if <i><b>'Voxel'</b><color=blue> isSolid</color></i>.");
                return voxelData.isSolid;
            }
        }

        public Vector2 GetTextureID(int faceIndex, Voxel voxelData) {
            switch(faceIndex) {
                case 0:
                    return WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].backFaceTexture;
                case 1:
                    return WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].frontFaceTexture;
                case 2:
                    return WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].topFaceTexture;
                case 3:
                    return WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].bottomFaceTexture;
                case 4:
                    return WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].leftFaceTexture;
                case 5:
                    return WorldMapData.dictionaryVoxelDefinition[voxelData.voxelTypeName].rightFaceTexture;
                default:
                    Debug.LogWarning("<b>GetTextureID</b> couldnt be <color=yellow><i>found</i></color>, <color=yellow><i>invalid Face Index</i></color>, returning <i>topFaceTexture</i> as <i>default</i>.");
                    return new Vector2(1, 1);
            }
        }

        public readonly Vector3[] neighbourChecks = new Vector3[24] {
            new Vector3(-voxelWidth, -voxelHeight, +voxelWidth), // Bottom NW
			new Vector3(0.0f, -voxelHeight, +voxelWidth), // Bottom N
			new Vector3(+voxelWidth, -voxelHeight, +voxelWidth), // Bottom NE
			new Vector3(+voxelWidth, -voxelHeight, 0.0f), // Bottom E
            new Vector3(+voxelWidth, -voxelHeight, -voxelWidth), // Bottom SE
            new Vector3(0.0f, -voxelHeight, -voxelWidth), // Bottom S
            new Vector3(-voxelWidth, -voxelHeight, -voxelWidth), // Bottom SW
            new Vector3(-voxelWidth, -voxelHeight, 0.0f), // Bottom W

            new Vector3(-voxelWidth, 0.0f, +voxelWidth), // NW
			new Vector3(0.0f, 0.0f, +voxelWidth), // N
			new Vector3(+voxelWidth, 0.0f, +voxelWidth), // NE
			new Vector3(+voxelWidth, 0.0f, 0.0f), // E
            new Vector3(+voxelWidth, 0.0f, -voxelWidth), // SE
            new Vector3(0.0f, 0.0f, -voxelWidth), // S
            new Vector3(-voxelWidth, 0.0f, -voxelWidth), // SW
            new Vector3(-voxelWidth, 0.0f, 0.0f), // W

            new Vector3(-voxelWidth, +voxelHeight, +voxelWidth), // Top NW
			new Vector3(0.0f, +voxelHeight, +voxelWidth), // Top N
			new Vector3(+voxelWidth, +voxelHeight, +voxelWidth), // Top NE
			new Vector3(+voxelWidth, +voxelHeight, 0.0f), // Top E
            new Vector3(+voxelWidth, +voxelHeight, -voxelWidth), // Top SE
            new Vector3(0.0f, +voxelHeight, -voxelWidth), // Top S
            new Vector3(-voxelWidth, +voxelHeight, -voxelWidth), // Top SW
            new Vector3(-voxelWidth, +voxelHeight, 0.0f), // Top W
        };
    }
}