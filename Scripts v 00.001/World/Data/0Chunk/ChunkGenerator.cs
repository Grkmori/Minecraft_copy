using UnityEngine;
using BlueBird.World;
using BlueBird.World.Parameters;
using BlueBird.World.Visual;

namespace BlueBird.World.Data {
    public sealed class ChunkGenerator {
        /* Instances */
        VoxelGenerator _voxelGenerator;
        VisualGenerator _visualGenerator;

        /* Variables - For Voxel */
        private float maxWidth = Constants_str.chunkWidth;
        private float maxHeight = Constants_str.chunkHeight;
        private float addWidth = Constants_str.voxelWidth;
        private float addHeight = Constants_str.voxelHeight;
        private int voxelCount;

        public void CreateChunks(Vector3 @baseVector, Vector2 @worldSizeVector, int @chunkWidth) {
            // Setting up
            _voxelGenerator = new VoxelGenerator();
            _visualGenerator = new VisualGenerator();

            Region2D @chunkRegions = new Region2D(@baseVector, @worldSizeVector); // Create World Regions for the Chunks for the use of "foreach" function
            GameObject @parentObject = GameObject.Find("WorldMap");

            foreach(Vector3 @position in @chunkRegions) {
                // Create a GameObject for the Chunk
                GameObject @chunkObject = new GameObject("Chunk " + @position, typeof(Chunk), typeof(MeshFilter), typeof(MeshRenderer)) {
                    tag = "Chunk",
                    layer = 10
                };
                @chunkObject.transform.SetParent(@parentObject.transform, true);
                @chunkObject.transform.localPosition = new Vector3((@position.x - 1) * @chunkWidth, 0, (@position.z - 1) * @chunkWidth);
                @chunkObject.transform.localScale = Vector3.one;
                @chunkObject.transform.localEulerAngles = Vector3.zero;

                // Creating Region2D for the Chunk and adding Data
                Region2D @region2D = new Region2D(new Vector3(((@position.x - 1) * @chunkWidth) + @baseVector.x, @baseVector.y, ((@position.z - 1) * @chunkWidth) + @baseVector.z), @chunkWidth);
                Chunk @chunkData = @chunkObject.GetComponent<Chunk>();
                @chunkData.position = @position;
                @chunkData._region2D = @region2D;
                @chunkData.dictionaryChunkVoxels.Clear(); // Clearing any Old Persistent Data from the Dictionary
                Debug.Log("<b>" + @chunkObject.name + " Region2D </b> was successful <color=green><i>generated</i></color>. Size: <i>" + @region2D.regionSize + "</i>.");

                // Creating Region3D for the Chunk and generating its respective Voxels and addint to the dictionaryChunkVoxels
                voxelCount = 0;
                for(float y = @baseVector.y; y <= maxHeight; y = y + addHeight) {
                    for(float x = @baseVector.x; x <= maxWidth; x = x + addWidth) {
                        for(float z = @baseVector.z; z <= maxWidth; z = z + addWidth) {
                            Vector3 @voxelPosition = new Vector3(x, y, z);
                            _voxelGenerator.CreateVoxelData(@voxelPosition, @chunkData);
                            voxelCount++;
                        }
                    }
                }
                if(@chunkData.dictionaryChunkVoxels.Count < voxelCount) {
                    Debug.LogWarning("<b>dictionaryChunkVoxels</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
                }
                Debug.Log("<b>Voxels</b> were <color=green><i>generated</i></color>. A total of: <i>" + voxelCount + "</i>. <b>dictionaryChunkVoxels</b> was <color=cyan><i>populated</i></color> with a total of <i>" + @chunkData.dictionaryChunkVoxels.Count + "</i>.");

                // Generate Chunk Mesh
                _visualGenerator.GenerateChunkMesh(@chunkObject, @chunkData);

                // Adding Chunks to the dictionaryChunks
                bool @tryAddFalse = WorldData.dictionaryChunks.TryAdd(@position, @chunkObject);
                if(!@tryAddFalse) {
                    Debug.LogError("<b>" + @chunkObject.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryChunks</b>.");
                }
            }
            if(WorldData.dictionaryChunks.Count < @worldSizeVector.x * @worldSizeVector.y) {
                Debug.LogWarning("<b>dictionaryChunks</b> had some problems and was <color=yellow><i>not fully populated</i></color>.");
            }
            Debug.Log("<b>Chunks</b> were <color=green><i>generated</i></color>. A total of: <i>" + @worldSizeVector.x * @worldSizeVector.y + "</i>. <b>dictionaryChunks</b> was <color=cyan><i>populated</i></color> with a total of <i>" + WorldData.dictionaryChunks.Count + "</i>.");
        }
    }
}