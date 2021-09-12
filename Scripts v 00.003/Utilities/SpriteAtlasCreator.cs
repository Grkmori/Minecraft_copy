using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEditor;
//using UnityEditor.AnimatedValues;
//using UnityEditor.UI;
//using UnityEditor.U2D;
//using UnityEditor.PackageManager;
using System.IO;
//using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlueBird.World.Utilities {
    public sealed class SpriteAtlasCreator {
        /* Variables */
        private readonly string spriteAtlasDestinationFolder = Application.dataPath + "/Resources/SpriteAtlas";
        private readonly string spriteSourceFolder = Application.dataPath + "/Resources/SpriteAtlas";

        public void CreateAtlasesFolder() {
            // Setting up
            DirectoryInfo @rootFolder = new DirectoryInfo(spriteSourceFolder);
            List<Texture2D> @listTextures2D = new List<Texture2D>();

            foreach(DirectoryInfo @rootSubFolder in @rootFolder.GetDirectories()) {
                // Check if Sprites are Packable and Add to the List
                @listTextures2D.Clear();
                foreach(FileInfo @spriteInfo in @rootSubFolder.GetFiles("*.png", SearchOption.AllDirectories)) {
                    string @spritePath = @spriteInfo.FullName;
                    string @spriteAssetPath = @spritePath.Substring(@spritePath.IndexOf("Assets"));
                    Texture2D @sprite = AssetDatabase.LoadAssetAtPath<Texture2D>(@spriteAssetPath) as Texture2D;
                    if(IsPackable(@sprite)) {
                        @listTextures2D.Add(@sprite);
                    }
                }

                // Create Sprite Atlas
                string @atlasName = @rootSubFolder.Name + ".spriteatlas";
                string @atlasMeta = @atlasName + ".meta";
                CreateSpriteAtlas(@atlasName, @atlasMeta);
                SpriteAtlas @spriteAtlas = Resources.Load<SpriteAtlas>("SpriteAtlas/" + @rootSubFolder.Name);
                AddPackSpriteAtlas(@spriteAtlas, @listTextures2D.ToArray());
                //PackSpriteAtlas(@spriteAtlas);
                AssetDatabase.SaveAssets();
                if(@spriteAtlas.spriteCount == 0) {
                    Debug.LogWarning("<b>" + @spriteAtlas.name + "</b> has been <color=green><i>created</i></color>, but <color=yellow><i>couldnt add</i></color> any Sprite: <i>" + @spriteAtlas.spriteCount + "</i> sprites.");
                } else {
                    Debug.Log("<b>" + @spriteAtlas.name + "</b> has been <color=green><i>created</i></color>, with a total of <i>" + @spriteAtlas.spriteCount + "</i> sprites.");
                }
            }
        }

        private bool IsPackable(Object @obj) {
            return @obj != null && (@obj.GetType() == typeof(Sprite) || @obj.GetType() == typeof(Texture2D) || (@obj.GetType() == typeof(DefaultAsset) && ProjectWindowUtil.IsFolder(@obj.GetInstanceID())));
        }

        private void AddPackSpriteAtlas(SpriteAtlas @atlas, Object[] @spt) {
            MethodInfo @methodInfo = System.Type
                 .GetType("UnityEditor.U2D.SpriteAtlasExtensions, UnityEditor")
                 .GetMethod("Add", BindingFlags.Public | BindingFlags.Static);
            if(@methodInfo != null) {
                @methodInfo.Invoke(null, new object[] { @atlas, @spt });
            }
            else {
                Debug.LogWarning("<b> Sprite Atlas " + @atlas.name + " MethodInfo</b> is <color=yellow><i>null</i></color>.");
            }
        }

        private void PackSpriteAtlas(SpriteAtlas @atlas) {
            MethodInfo @methodInfo = System.Type
                .GetType("UnityEditor.U2D.SpriteAtlasUtility, UnityEditor")
                .GetMethod("PackAtlases", BindingFlags.NonPublic | BindingFlags.Static);
            @methodInfo.Invoke(null, new object[] { new[] { @atlas }, EditorUserBuildSettings.activeBuildTarget });
        }

        private void CreateSpriteAtlas(string @atlasName, string @atlasMeta) {
            string @yaml = @"%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!687078895 &4343727234628468602
SpriteAtlas:
  m_ObjectHideFlags: 0
  m_PrefabParentObject: {fileID: 0}
  m_PrefabInternal: {fileID: 0}
  m_Name: New Sprite Atlas
  m_EditorData:
    textureSettings:
      serializedVersion: 2
      anisoLevel: 1
      compressionQuality: 50
      maxTextureSize: 8192
      textureCompression: 0
      filterMode: 1
      generateMipMaps: 0
      readable: 0
      crunchedCompression: 0
      sRGB: 1
    platformSettings: []
    packingParameters:
      serializedVersion: 2
      padding: 4
      blockOffset: 1
      allowAlphaSplitting: 0
      enableRotation: 1
      enableTightPacking: 1
    variantMultiplier: 1
    packables: []
    bindAsDefault: 1
  m_MasterAtlas: {fileID: 0}
  m_PackedSprites: []
  m_PackedSpriteNamesToIndex: []
  m_Tag: New Sprite Atlas
  m_IsVariant: 0
";
            AssetDatabase.Refresh();

            // Check if Sprite Atlas Destination Folder exists
            if(!Directory.Exists(spriteAtlasDestinationFolder)) {
                Directory.CreateDirectory(spriteAtlasDestinationFolder);
                AssetDatabase.Refresh();
            }

            // Check if Sprite Atlas already exists, if so Delete
            string @fileAtlasPath = spriteAtlasDestinationFolder + "/" + @atlasName;
            string @fileMetaPath = spriteAtlasDestinationFolder + "/" + @atlasMeta;
            if(File.Exists(@fileAtlasPath)) {
                File.Delete(@fileAtlasPath);
                File.Delete(@fileMetaPath);
                AssetDatabase.Refresh();
            }

            // Create Sprite Atlas File
            FileStream @fs = new FileStream(@fileAtlasPath, FileMode.CreateNew);
            byte[] @bytes = new UTF8Encoding().GetBytes(@yaml);
            @fs.Write(@bytes, 0, @bytes.Length);
            @fs.Close();
            AssetDatabase.Refresh();
        }
    }
}