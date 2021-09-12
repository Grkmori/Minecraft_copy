using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class ChunkGenerator {
        /* Instances */
        VoxelGenerator _voxelGenerator = new VoxelGenerator();

        /* Variables - For Chunk */
        private int chunkCount;
        private Vector3 position;

        /* Variables - For Voxel */
        private int voxelCount;
        private int totalVoxelsChunksDictionaries = 0;
        private bool voxelTotalNumberError = false;

        public void SetupChunkGameObject(GameObject @chunkObject, Vector3 @position, Vector2 @chunkSize, Vector2 @voxelSize) {
            // Setting up
            chunkCount++;

            // Setup Chunk and Create Region2D and Region3D for ChunkData
            @chunkObject.name = "Chunk " + position;
            Region2D @region2D = new Region2D(new Vector2(@chunkObject.transform.position.x, @chunkObject.transform.position.z), @chunkSize.x, @voxelSize.x);
            Region3D @region3D = new Region3D(@chunkObject.transform.position, @chunkSize, @voxelSize);
            Chunk @chunkData = @chunkObject.GetComponent<Chunk>();
            @chunkData.chunkPosition = position;
            @chunkData.chunkObjectPosition = @chunkObject.transform.localPosition;
            @chunkData._chunkBounds = new Bounds(new Vector3(@chunkData.chunkObjectPosition.x, chunkSize.y / 2, @chunkData.chunkObjectPosition.y), new Vector3(@chunkSize.x + voxelSize.x, chunkSize.y + voxelSize.y, @chunkSize.x + voxelSize.x));
            @chunkData._region2DQuads = @region2D;
            @chunkData._region3DVoxels = @region3D;
            @chunkData.SetVisible(false);
            ///Debug.Log("<b>Chunk Data " + @chunkData.chunkPosition + "</b> was successfully <color=green><i>generated</i></color>. " +
            ///          "Size: <i>" + @region2D.regionSize + " Quads</i>; Size: <i>" + @region3D.regionSize + " Voxels</i>; " +
            ///          "CenterVector2: <i>" + @chunkData._chunkBounds.center + "</i>; MinVector2: <i>" + @chunkData._chunkBounds.min + "</i>; MaxVector2: <i>" + @chunkData._chunkBounds.max + "</i>.");

            // Adding ChunkObject to the dictionaryChunkObject
            bool @tryAddFalseObject = WorldData.dictionaryChunkObject.TryAdd(position, @chunkObject);
            if(!@tryAddFalseObject) {
                Debug.LogError("<b>" + @chunkObject.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkObject</b>.");
            }
            // Adding ChunkData to the dictionaryChunkData
            bool @tryAddFalseData = WorldData.dictionaryChunkData.TryAdd(position, @chunkData);
            if(!@tryAddFalseData) {
                Debug.LogError("<b>Chunk " + @chunkData.chunkPosition + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunkData</b>.");
            }
        }

        public void CreateChunksVoxels(Vector3 @position, Vector2 @voxelSize) {
            // Use World Region2DChunks to Create Chunks Voxels for each 'position' of the World
            voxelCount = 0;

            Chunk @chunkData = WorldData.dictionaryChunkData[@position];
            Region3D @chunkRegion3D = @chunkData._region3DVoxels;

            // Use Region3D to Create Voxels for each 'position' of the Chunk
            foreach(Vector3 @voxelPosition in @chunkRegion3D) {
                _voxelGenerator.CreateVoxelData(@voxelPosition, @chunkData, @voxelSize);
                voxelCount++;
            }
            if(@chunkData.dictionaryChunkVoxels.Count < voxelCount) {
                Debug.LogWarning("<b>dictionaryChunkVoxels</b> from <b>Chunk " + @chunkData.chunkPosition + "</b>  had some problems and <color=yellow><i>was not fully populated</i></color>.");
                voxelTotalNumberError = true;
            } else {
                ///Debug.Log("<b>Voxels</b> from <b>Chunk " + @chunkData.chunkPosition + "</b>  were <color=green><i>generated</i></color>. " +
                ///          "A total of: <i>" + voxelCount + "</i> Voxels. <b>dictionaryChunkVoxels</b> was <color=cyan><i>populated</i></color> with a total of <i>" + @chunkData.dictionaryChunkVoxels.Count + " Voxels</i>.");
            }
        }

        public void ChunksFinalCheck(Region2D @worldChunks, Vector3 @baseVector, Vector2 @worldSize) {
            // Check for Success on dictionaryChunkObject
            if(WorldData.dictionaryChunkObject.Count < (@worldSize.x) * (@worldSize.y)) {
                Debug.LogWarning("<b>dictionaryChunkObject</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Chunks Objects</b> were <color=green><i>generated</i></color>. " +
                          "A total of: <i>" + (@worldSize.x) * (@worldSize.y) + " ChunkObjects</i>. <b>dictionaryChunkObject</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryChunkObject.Count + " ChunkObjects</i>.");
            }

            // Check for Success on dictionaryChunkData
            if(WorldData.dictionaryChunkData.Count < @worldSize.x * @worldSize.y) {
                Debug.LogWarning("<b>dictionaryChunkData</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            } else {
                Debug.Log("<b>Chunks Data</b> were <color=green><i>generated</i></color>. A total of: <i>" + @worldSize.x * @worldSize.y + " ChunkData</i>. " +
                          "<b>dictionaryChunkData</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryChunkData.Count + " ChunkDatas</i>.");
            }

            // Final check if Voxels were successfully Added to all dictionaryChunkVoxels
            foreach(Vector2 @chunk2DPosition in @worldChunks) {
                position = new Vector3(@chunk2DPosition.x, @baseVector.y, @chunk2DPosition.y);
                Chunk @chunkDataCount = WorldData.dictionaryChunkData[position];
                totalVoxelsChunksDictionaries += @chunkDataCount.dictionaryChunkVoxels.Count;
            }
            if(voxelTotalNumberError) {
                Debug.LogWarning("<b>Generating Voxels</b> had some problems and <color=yellow><i>were not fully generated</i></color>. " +
                                 "A total of: <i>" + voxelCount * chunkCount + " Voxels </i> were expected but only: <i>" + @totalVoxelsChunksDictionaries + " Voxels</i> were added to the <b>Chunk Dictionaries dictionaryChunkVoxels</b>.");
            } else {
                Debug.Log("<b>Generating Voxels</b> was <color=green><i>successful</i></color>. " +
                          "A total of: <i>" + voxelCount * chunkCount + " Voxels </i>. <b>Chunk Voxels Dictionaries</b> were <color=cyan><i>populated</i></color> with a total of <i>" + totalVoxelsChunksDictionaries + " Voxels</i>.");
            }
        }
    }
}