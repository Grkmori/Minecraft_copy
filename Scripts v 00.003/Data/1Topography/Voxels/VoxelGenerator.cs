using UnityEngine;
using BlueBird.World.Definitions;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Noises;

namespace BlueBird.World.Data.Topography.Voxels {
    public sealed class VoxelGenerator {
        /* Instances */
        NoiseGenerator _noiseGenerator = new NoiseGenerator();

        /* Variables - For Voxels */
        private string voxelType;
        private string biomeType;
        private int terrainHeight;

        public void CreateVoxelData(Vector3 @position, Chunk @chunkData, Vector2 @voxelSize) {
            // Generating and Storing Voxels in the Chunk and adding Data
            Voxel @voxel = new Voxel();
            @voxel.voxelPosition = @position;
            @voxel.voxelTypeName = GetVoxelTypeName(@position);

            // Adding Voxels to the dictionaryChunkVoxels and dictionaryVoxelData
            bool @tryAddFalseChunkVoxels = @chunkData.dictionaryChunkVoxels.TryAdd(@voxel.voxelPosition, @voxel);
            if(!@tryAddFalseChunkVoxels) {
                Debug.LogError("<b>Voxel " + @voxel.voxelPosition + "</b> from <b>Chunk " + @chunkData.chunkPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkVoxels</b>.");
            }
        }

        private string GetVoxelTypeName(Vector3 @position) {
            // Setting up
            biomeType = "Default"; // TODO: To be dynamically Set
            BiomeDefinition @biome = WorldData.dictionaryBiomeDefinition[biomeType];
            terrainHeight = _noiseGenerator.TerrainNoise(@position, @biome);

            // Get VoxelTypeName for the identity of the Voxel
            /// First Pass - Basic
            if(@position.y == 0) {
                voxelType = "Bedrock";
            } else if(@position.y == terrainHeight) {
                voxelType = "Grass";
            } else if(position.y < terrainHeight && position.y > terrainHeight - 3) {
                voxelType = "Dirt";
            } else if(@position.y > 0 && @position.y <= terrainHeight - 3){
                voxelType = "Stone";
            } else if(position.y > terrainHeight) {
                voxelType = "Air";
            } else {
                voxelType = null;
            }

            /// Second Pass - Lodes
            foreach(string @biomeLodeName in @biome.lode) {
                BiomeLodeDefinition @biomeLode = WorldData.dictionaryBiomeLodeDefinition[@biomeLodeName];
                if(@biomeLodeName == "Sand") {
                    if(voxelType == "Dirt" || voxelType == "Grass") {
                        if(position.y > @biomeLode.minHeight && position.y < @biomeLode.maxHeight) {
                            if(_noiseGenerator.Noise3D(@position, @biomeLode.scale, @biomeLode.threshold)) {
                                voxelType = @biomeLode.blockType;
                            }
                        }
                    }
                } else if(@biomeLodeName == "Cobblestone") {
                    if(voxelType == "Stone") {
                        if(position.y > @biomeLode.minHeight && position.y < @biomeLode.maxHeight) {
                            if(_noiseGenerator.Noise3D(@position, @biomeLode.scale, @biomeLode.threshold)) {
                                voxelType = @biomeLode.blockType;
                            }
                        }
                    }
                }
            }

            return voxelType;
        }
    }
}
