using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Concurrent;

namespace BlueBird.World.Visual {
    public sealed class VisualData {
		/* Visual Resource Storage */
		public static ConcurrentDictionary<string, SpriteAtlas> dictionarySpriteAtlases;
		public static ConcurrentDictionary<string, Material> dictionaryMaterials;
		public static ConcurrentDictionary<string, Texture2D> dictionaryTextures;
		public static ConcurrentDictionary<string, Sprite> dictionarySprites;
		public static ConcurrentDictionary<string, GameObject> dictionaryPrefabs;
		//public static ConcurrentDictionary<Color, Texture2D> unicolorTextures = new Dictionary<Color, Texture2D>();

		/* Visual Meshes Storage */
		public static ConcurrentDictionary<Vector3, Mesh> dictionaryChunkMesh;
	}
}