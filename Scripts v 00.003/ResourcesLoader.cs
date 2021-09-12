using UnityEngine;
using UnityEngine.U2D;
using BlueBird.World.Visual;

namespace BlueBird.World {
	public sealed class ResourcesLoader {
		public void LoadWorldResources() {
			foreach(GameObject @prefab in Resources.LoadAll<GameObject>("Prefabs/Static/World")) {
				bool tryAddFalse = WorldData.dictionaryStaticWorldPrefabs.TryAdd(@prefab.name, @prefab);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Prefab " + @prefab.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryStaticWorldPrefabs</b>.");
				}
			}
			if(WorldData.dictionaryStaticWorldPrefabs.Count == 0) {
				Debug.LogWarning("<b>dictionaryStaticWorldPrefabs</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Prefabs</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryStaticWorldPrefabs</b> with a total of <i>" + WorldData.dictionaryStaticWorldPrefabs.Count + " Prefabs</i>.");
			}

			foreach(GameObject @prefab in Resources.LoadAll<GameObject>("Prefabs/Dynamic/")) {
				bool tryAddFalse = WorldData.dictionaryDynamicPrefabs.TryAdd(@prefab.name, @prefab);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Prefab " + @prefab.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryDynamicPrefabs</b>.");
				}
			}
			if(WorldData.dictionaryDynamicPrefabs.Count == 0) {
				Debug.LogWarning("<b>dictionaryDynamicPrefabs</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Prefabs</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryDynamicPrefabs</b> with a total of <i>" + WorldData.dictionaryDynamicPrefabs.Count + " Prefabs</i>.");
			}

			foreach(GameObject @prefab in Resources.LoadAll<GameObject>("Prefabs/Player/Player")) {
				bool tryAddFalse = WorldData.dictionaryPlayerPrefabs.TryAdd(@prefab.name, @prefab);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Prefab " + @prefab.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerPrefabs</b>.");
				}
			}
			if(WorldData.dictionaryPlayerPrefabs.Count == 0) {
				Debug.LogWarning("<b>dictionaryPlayerPrefabs</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Prefabs</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryPlayerPrefabs</b> with a total of <i>" + WorldData.dictionaryPlayerPrefabs.Count + " Prefabs</i>.");
			}

			foreach(GameObject @prefab in Resources.LoadAll<GameObject>("Prefabs/Player/UI/")) {
				bool tryAddFalse = WorldData.dictionaryPlayerUIPrefabs.TryAdd(@prefab.name, @prefab);
				if(!@tryAddFalse) {
					Debug.LogError("<b>Prefab " + @prefab.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryPlayerUIPrefabs</b>.");
				}
			}
			if(WorldData.dictionaryPlayerPrefabs.Count == 0) {
				Debug.LogWarning("<b>dictionaryPlayerUIPrefabs</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>Prefabs</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryPlayerUIPrefabs</b> with a total of <i>" + WorldData.dictionaryPlayerUIPrefabs.Count + " Prefabs</i>.");
			}
		}

		public void LoadVisualResources() {
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
				Debug.Log("<b>SpriteAtlases</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionarySpriteAtlases</b> with a total of <i>" + VisualData.dictionarySpriteAtlases.Count + " SpriteAtlases</i>.");
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
				Debug.Log("<b>Materials</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryMaterials</b> with a total of <i>" + VisualData.dictionaryMaterials.Count + " Materials</i>.");
			}

			foreach(PhysicMaterial @physicMaterial in Resources.LoadAll<PhysicMaterial>("PhysicMaterials/")) {
				bool tryAddFalse = VisualData.dictionaryPhysicMaterials.TryAdd(@physicMaterial.name, @physicMaterial);
				if(!@tryAddFalse) {
					Debug.LogError("<b>PhysicMaterial " + @physicMaterial.name + "</b> <color=red><i>failed to add</i></color> to <b>dictionaryPhysicMaterials</b>.");
				}
			}
			if(VisualData.dictionaryPhysicMaterials.Count == 0) {
				Debug.LogWarning("<b>dictionaryPhysicMaterials</b> had some problems and <color=yellow><i>was not fully populated</i></color>.");
			} else {
				Debug.Log("<b>PhysicMaterials</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> the <b>dictionaryPhysicMaterials</b> with a total of <i>" + VisualData.dictionaryPhysicMaterials.Count + " PhysicMaterials</i>.");
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
				Debug.Log("<b>Textures</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionaryTextures</b> with a total of <i>" + VisualData.dictionaryTextures.Count + " Textures</i>.");
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
				Debug.Log("<b>Sprites</b> were successfully <i><color=green>loaded</color> and <color=cyan>populated</color></i> to the <b>dictionarySprites</b> with a total of <i>" + VisualData.dictionarySprites.Count + " Sprites</i>.");
			}
		}
	}
}