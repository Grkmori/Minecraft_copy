using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Concurrent;
using BlueBird.World;
using BlueBird.World.Utilities;
using BlueBird.World.Data;

namespace BlueBird.World.Visual {
    public sealed class VisualGenerator {
        /* Instances */
        SpriteAtlasCreator _spriteAtlasCreator;
        StaticMeshesGenerator _staticMeshesGenerator;

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
            Debug.Log("<b>ResourcesData Dictionaries</b> were successful <color=green><i>generated</i></color>.");

            // Clearing any Old Persistent Data from the Dictionaries
            VisualData.dictionarySpriteAtlases.Clear();
            VisualData.dictionaryMaterials.Clear();
            VisualData.dictionaryTextures.Clear();
            VisualData.dictionarySprites.Clear();
            VisualData.dictionaryPrefabs.Clear();
            Debug.Log("<b>Old Persitent Data</b> from <b>VisualData Dictionaries</b> were successful <color=cyan><i>cleared</i></color> if they had any.");
        }

        public void GenerateChunkMesh(GameObject @chunkObject, Chunk @chunkData) {
            // Setting up
            _staticMeshesGenerator = new StaticMeshesGenerator();

            // Generating Chunks Meshes
            _staticMeshesGenerator.CreateChunksData(@chunkData);
            _staticMeshesGenerator.CreateChunksMesh(@chunkObject, @chunkData._meshData);
        }
    }
}