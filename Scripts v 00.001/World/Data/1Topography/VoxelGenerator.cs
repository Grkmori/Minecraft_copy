using UnityEngine;
using BlueBird.World.Parameters;

namespace BlueBird.World.Data {
    public sealed class VoxelGenerator {
        public void CreateVoxelData(Vector3 @position, Chunk @chunkData) {
            // Generating and Storing Voxels in the Chunk and adding Data
            Voxel @voxel = new Voxel();
            @voxel.position = @position;
            if(@position.y == 1) {
                @voxel.voxelTypeName = "Bedrock"; // TODO: value will be defined dynamically
            } else if(@position.y > 1 && @position.y < Constants_str.chunkHeight) {
                @voxel.voxelTypeName = "Stone"; // TODO: value will be defined dynamically
            } else if(@position.y == Constants_str.chunkHeight) {
                @voxel.voxelTypeName = "Grass"; // TODO: value will be defined dynamically
            }

            // Adding Voxels to the dictionaryChunkVoxels
            bool @tryAddFalse = @chunkData.dictionaryChunkVoxels.TryAdd(@position, @voxel);
            if(!@tryAddFalse) {
                Debug.LogError("<b>Voxel " + @position + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkVoxels</b>.");
            }
        }
    }
}
