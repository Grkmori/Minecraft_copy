using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Concurrent;

namespace BlueBird.World.Visual {
    public sealed class ResourcesLoader {
		public void LoadResources() {
			// Load Resources into Dictionaries
			foreach(SpriteAtlas @spriteAtlas in Resources.LoadAll<SpriteAtlas>("SpriteAtlas/")) {
				bool tryAddFalse = VisualData.dictionarySpriteAtlases.TryAdd(@spriteAtlas.name, @spriteAtlas);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Sprite Atlas " + @spriteAtlas.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionarySpriteAtlases</b>.");
				}
			}
			if(VisualData.dictionarySpriteAtlases.Count == 0) {
				Debug.LogWarning("<b>dictionarySpriteAtlases</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>SpriteAtlases</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionarySpriteAtlases</b> with a total of <i>" + VisualData.dictionarySpriteAtlases.Count + " SpriteAtlases</i>.");
			}

			foreach(Material @material in Resources.LoadAll<Material>("Materials/")) {
				bool tryAddFalse = VisualData.dictionaryMaterials.TryAdd(@material.name, @material);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Material " + @material.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryMaterials</b>.");
				}
			}
			if(VisualData.dictionaryMaterials.Count == 0) {
				Debug.LogWarning("<b>dictionaryMaterials</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Materials</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryMaterials</b> with a total of <i>" + VisualData.dictionaryMaterials.Count + " Materials</i>.");
			}

			foreach(Texture2D @texture in Resources.LoadAll<Texture2D>("Textures/")) {
				bool tryAddFalse = VisualData.dictionaryTextures.TryAdd(@texture.name, @texture);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Texture " + @texture.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryTextures</b>.");
				}
			}
			if(VisualData.dictionaryTextures.Count == 0) {
				Debug.LogWarning("<b>dictionaryTextures</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Textures</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryTextures</b> with a total of <i>" + VisualData.dictionaryTextures.Count + " Textures</i>.");
			}

			foreach(Sprite @sprite in Resources.LoadAll<Sprite>("Sprites/")) {
				bool tryAddFalse = VisualData.dictionarySprites.TryAdd(@sprite.name, @sprite);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Sprite " + @sprite.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionarySprites</b>.");
				}
			}
			if(VisualData.dictionarySprites.Count == 0) {
				Debug.LogWarning("<b>dictionarySprites</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Sprites</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionarySprites</b> with a total of <i>" + VisualData.dictionarySprites.Count + " Sprites</i>.");
			}

			foreach(GameObject @prefab in Resources.LoadAll<GameObject>("Prefabs/")) {
				bool tryAddFalse = VisualData.dictionaryPrefabs.TryAdd(@prefab.name, @prefab);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Prefab " + @prefab.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryPrefabs</b>.");
				}
			}
			if(VisualData.dictionaryPrefabs.Count == 0) {
				Debug.LogWarning("<b>dictionaryPrefabs</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Prefabs</b> were successful <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryPrefabs</b> with a total of <i>" + VisualData.dictionaryPrefabs.Count + " Prefabs</i>.");
			}
		}
	}
}