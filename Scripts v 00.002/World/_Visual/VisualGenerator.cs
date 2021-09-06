using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Concurrent;
using BlueBird.World;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Data.InvisibleData.Chunks;
using BlueBird.World.Data.Topography.Voxels;

namespace BlueBird.World.Visual {
    public sealed class VisualGenerator {
        /* Instances */
        Transform playerTransform;

        SpriteAtlasCreator _spriteAtlasCreator;

        ChunkMeshGenerator _chunkMeshGenerator = new ChunkMeshGenerator();

        public void CreateSpriteAtlases() {
            // Setting up
            _spriteAtlasCreator = new SpriteAtlasCreator();

            // Create Sprite Atlases
            _spriteAtlasCreator.CreateAtlasesFolder();
        }

        public void CreateVisualDictionaries() {
            // Create Resources Data Dictionaries
            VisualData.dictionarySpriteAtlases = new ConcurrentDictionary<string, SpriteAtlas>();
            VisualData.dictionaryMaterials = new ConcurrentDictionary<string, Material>();
            VisualData.dictionaryTextures = new ConcurrentDictionary<string, Texture2D>();
            VisualData.dictionarySprites = new ConcurrentDictionary<string, Sprite>();
            VisualData.dictionaryPrefabs = new ConcurrentDictionary<string, GameObject>();
            Debug.Log("<b>ResourcesData Dictionaries</b> were successful <color=green><i>created</i></color>.");

            // Create Visual Meshes Dictionaries
            VisualData.dictionaryChunkMesh = new ConcurrentDictionary<Vector3, Mesh>();
            Debug.Log("<b>MeshesData Dictionaries</b> were successful <color=green><i>created</i></color>.");
        }

        public void ClearVisualDictionaries() {
            // Clearing any Old Persistent from VisualData Dictionaries
            VisualData.dictionarySpriteAtlases.Clear();
            VisualData.dictionaryMaterials.Clear();
            VisualData.dictionaryTextures.Clear();
            VisualData.dictionarySprites.Clear();
            VisualData.dictionaryPrefabs.Clear();

            VisualData.dictionaryChunkMesh.Clear();

            Debug.Log("<b>Old Persitent Data</b> from <b>VisualData Dictionaries</b> were successful <color=cyan><i>cleared</i></color> if they had any.");
        }
    }
}