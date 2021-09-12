using UnityEngine;
using BlueBird.World.Data.InvisibleData.Chunks;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelUtilities {
        /* Instances */
        ChunkUtilities _chunkUtilities = new ChunkUtilities();

        public Vector3 GetVoxelPositionFromPosition(Vector3 @position) {
            float x = Mathf.RoundToInt(@position.x);
            float y = Mathf.RoundToInt(@position.y);
            float z = Mathf.RoundToInt(@position.z);
            return new Vector3(x, y, z);
        }

        public bool CheckFacesOutOfChunk(Vector3 @voxelFacePosition, Voxel @voxelData, out Voxel @cursorFaceVoxel) {
            Vector3 @chunkDataPosition = _chunkUtilities.GetChunkPositionFromPosition(@voxelData.voxelPosition);
            Vector3 @chunkFacePosition = _chunkUtilities.GetChunkPositionFromPosition(@voxelFacePosition);

            if(WorldData.dictionaryChunkData.ContainsKey(@chunkFacePosition)) {
                if(!WorldData.dictionaryChunkData[@chunkDataPosition].dictionaryChunkVoxels.ContainsKey(@voxelFacePosition)) {
                    @cursorFaceVoxel = WorldData.dictionaryChunkData[@chunkFacePosition].dictionaryChunkVoxels[@voxelFacePosition];
                    return true;
                } else {
                    @cursorFaceVoxel = voxelData; // Just to return some value to 'out' parameter, this wont be used (because of the 'if statement' on the calling function)
                    return false;
                }
            } else {
                @cursorFaceVoxel = voxelData; // Just to return some value to 'out' parameter, this wont be used (because of the 'if statement' on the calling function)
                return false;
            }
        }

        public bool CheckVoxelFace(Vector3 @facePosition, Vector3 @chunkPosition, Chunk @chunkData, Voxel @voxelData) {
            // Setting up
            Vector3 @voxelFacePosition = @facePosition + @chunkPosition;
            Vector3 @chunkFacePosition = _chunkUtilities.GetChunkPositionFromPosition(@voxelFacePosition);

            // Checking if Voxel "Faces"(Voxel next to the current Voxel) to be Rendered or Not
            if(WorldData.dictionaryChunkData.ContainsKey(@chunkFacePosition)) {
                if(@chunkData.dictionaryChunkVoxels.ContainsKey(@voxelFacePosition)) {
                    ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "<b> is <color=orange><i>inside of 'Chunk Bounds'</i></color>, return <i><b>'VoxelFace'</b><color=blue> isTransparent</color></i>.");
                    return WorldData.dictionaryVoxelDefinition[@chunkData.dictionaryChunkVoxels[@voxelFacePosition].voxelTypeName].isTransparent;
                } else {
                    if(@voxelFacePosition.y < 0) {
                        ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "</b> is <color=orange><i>under(y < 0) 'Chunk Bounds'</i></color>, return <i><color=blue>False</color></i>.");
                        return false;
                    } else {
                        ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "</b> is <color=orange><i>out of 'Chunk Bounds'</i></color>, return if <i><b>'VoxelFace'</b><color=blue> isTransparent</color></i>.");
                        return WorldData.dictionaryVoxelDefinition[WorldData.dictionaryChunkData[@chunkFacePosition].dictionaryChunkVoxels[@voxelFacePosition].voxelTypeName].isTransparent;
                    }
                }
            } else {
                ///Debug.Log("If <b>'VoxelFace' " + @voxelFacePosition + "</b> is <color=orange><i>out of 'World Bounds'</i></color>, return if <i><b>'Voxel'</b><color=blue> isSolid</color></i>.");
                return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].isSolid;
            }
        }

        public Vector2 GetTextureID(int @faceIndex, Voxel @voxelData) {
            switch(@faceIndex) {
                case 0:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].backFaceTexture;
                case 1:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].frontFaceTexture;
                case 2:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].topFaceTexture;
                case 3:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].bottomFaceTexture;
                case 4:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].leftFaceTexture;
                case 5:
                    return WorldData.dictionaryVoxelDefinition[@voxelData.voxelTypeName].rightFaceTexture;
                default:
                    Debug.LogWarning("<b>GetTextureID</b> couldnt be <color=yellow><i>found</i></color>, <color=yellow><i>invalid Face Index</i></color>, returning <i>topFaceTexture</i> as <i>default</i>.");
                    return new Vector2(1, 1);
            }
        }
    }
}