using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Concurrent;

namespace BlueBird.World.Visual {
    public sealed class ResourcesLoader {
		/* Instances */

		public void LoadResources() {
			// Load Resources into Dictionaries
			foreach(SpriteAtlas @spriteAtlas in Resources.LoadAll<SpriteAtlas>("SpriteAtlas/")) {
				bool tryAddFalse = VisualData.dictionarySpriteAtlases.TryAdd(@spriteAtlas.name, @spriteAtlas);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Sprite Atlas " + @spriteAtlas.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionarySpriteAtlases</b>.");
				}
			}
			Debug.Log("<b>SpriteAtlases</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionarySpriteAtlases</b> with a total of <i>" + VisualData.dictionarySpriteAtlases.Count + "</i>.");

			foreach(Material @material in Resources.LoadAll<Material>("Materials/")) {
				bool tryAddFalse = VisualData.dictionaryMaterials.TryAdd(@material.name, @material);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Material " + @material.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryMaterials</b>.");
				}
			}
			Debug.Log("<b>Materials</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryMaterials</b> with a total of <i>" + VisualData.dictionaryMaterials.Count + "</i>.");

			foreach(Texture2D @texture in Resources.LoadAll<Texture2D>("Textures/")) {
				bool tryAddFalse = VisualData.dictionaryTextures.TryAdd(@texture.name, @texture);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Texture " + @texture.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryTextures</b>.");
				}
			}
			Debug.Log("<b>Textures</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryTextures</b> with a total of <i>" + VisualData.dictionaryTextures.Count + "</i>.");

			foreach(Sprite @sprite in Resources.LoadAll<Sprite>("Sprites/")) {
				bool tryAddFalse = VisualData.dictionarySprites.TryAdd(@sprite.name, @sprite);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Sprite " + @sprite.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionarySprites</b>.");
				}
			}
			Debug.Log("<b>Sprites</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionarySprites</b> with a total of <i>" + VisualData.dictionarySprites.Count + "</i>.");

			foreach(GameObject @prefab in Resources.LoadAll<GameObject>("Prefabs/")) {
				bool tryAddFalse = VisualData.dictionaryPrefabs.TryAdd(@prefab.name, @prefab);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Prefab " + @prefab.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryPrefabs</b>.");
				}
			}
			Debug.Log("<b>Prefabs</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryPrefabs</b> with a total of <i>" + VisualData.dictionaryPrefabs.Count + "</i>.");
		}
	}
}