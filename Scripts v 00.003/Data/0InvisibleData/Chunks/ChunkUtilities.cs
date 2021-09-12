using UnityEngine;
using BlueBird.World.Parameters;
using BlueBird.World.Data.Topography.Voxels;


namespace BlueBird.World.Data.InvisibleData.Chunks {
    public sealed class ChunkUtilities {
        /* Variables - For  Chunks */
        private readonly float chunkPosY = Constants_str.worldBaseVector3.y;
        private readonly float chunkWidth = Constants_str.chunkSize.x;
        private readonly float chunkCorrection = 0.01f;

        public Vector3 GetChunkPositionFromPosition(Vector3 @position) {
            float x = Mathf.RoundToInt((@position.x + chunkCorrection) / chunkWidth);
            float y = chunkPosY;
            float z = Mathf.RoundToInt((@position.z + chunkCorrection) / chunkWidth);
            return new Vector3(x, y, z);
        }

        public Chunk CheckForCurrentChunk(Vector3 @characterPosition, Chunk @currentChunk) {
            // Check and Return current Character Chunk if had any changes
            Vector3 characterChunkPosition = GetChunkPositionFromPosition(@characterPosition);
            if(@currentChunk.chunkPosition != characterChunkPosition) {
                if(WorldData.dictionaryChunkData.ContainsKey(characterChunkPosition)) {
                    @currentChunk = WorldData.dictionaryChunkData[characterChunkPosition];
                } else {
                    Debug.LogWarning("<b>Character/Interaction</b> is <color=yellow><i>out of World Bounds</i></color>.");
                }
            }

            return @currentChunk;
        }

        public void ClearChunkMeshData(Chunk @chunkData) {
            @chunkData._chunkMeshData.listVertices.Clear();
            @chunkData._chunkMeshData.listTrianglesIndices.Clear();
            @chunkData._chunkMeshData.listTransparentTrianglesIndices.Clear();
            @chunkData._chunkMeshData.listUVs.Clear();
        }
    }
}