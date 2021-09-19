using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Concurrent;
using BlueBird.World.Utilities;

namespace BlueBird.World.Visual {
    public sealed class VisualGenerator {
        /* Instances */
        SpriteAtlasCreator _spriteAtlasCreator;

        public void CreateSpriteAtlases() {
            // Setting up
            _spriteAtlasCreator = new SpriteAtlasCreator();

            // Create Sprite Atlases
            _spriteAtlasCreator.CreateAtlasesFolder();
        }

        #region Dictionaries
        public void CreateVisualDictionaries() {
            // Create Resources Data Dictionaries
            VisualData.dictionarySpriteAtlases = new ConcurrentDictionary<string, SpriteAtlas>();
            VisualData.dictionaryMaterials = new ConcurrentDictionary<string, Material>();
            VisualData.dictionaryPhysicMaterials = new ConcurrentDictionary<string, PhysicMaterial>();
            VisualData.dictionaryTextures = new ConcurrentDictionary<string, Texture2D>();
            VisualData.dictionarySprites = new ConcurrentDictionary<string, Sprite>();
            Debug.Log("<b>ResourcesData Dictionaries</b> were successfully <color=green><i>created</i></color>.");

            // Create Visual Meshes Dictionaries
            VisualData.dictionaryChunkMesh = new ConcurrentDictionary<Vector3, Mesh>();
            Debug.Log("<b>MeshesData Dictionaries</b> were successfully <color=green><i>created</i></color>.");
        }

        public void ClearVisualDictionaries() {
            // Clearing any Old Persistent from VisualData Dictionaries
            VisualData.dictionarySpriteAtlases.Clear();
            VisualData.dictionaryMaterials.Clear();
            VisualData.dictionaryPhysicMaterials.Clear();
            VisualData.dictionaryTextures.Clear();
            VisualData.dictionarySprites.Clear();

            VisualData.dictionaryChunkMesh.Clear();

            Debug.Log("<b>Old Persistent Data</b> from <b>VisualData Dictionaries</b> were successfully <color=cyan><i>cleared</i></color> if they had any.");
        }
        #endregion
    }
}