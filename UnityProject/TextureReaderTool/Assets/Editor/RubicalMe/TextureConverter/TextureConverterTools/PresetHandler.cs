using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace RubicalMe
{
	namespace TextureConverterTools
	{
		public static class PresetHandler
		{

			public static TextureConverterPreset CreatePreset ()
			{
				TextureConverterPreset asset = ScriptableObject.CreateInstance<TextureConverterPreset> ();

				AssetDatabase.CreateAsset (asset, "Assets/Tools/RubicalMe/ConverterPresets/NewPreset.asset");
				AssetDatabase.SaveAssets ();

				return asset;
			}

			public static void SavePreset (TextureConverterPreset preset)
			{
				Debug.Log ("should be saved");
				CreateOrReplaceAsset (preset, "Assets/Tools/RubicalMe/ConverterPresets/" + preset.name + ".asset");
			}

			public static List<string> LoadPresets ()
			{
				if (!AssetDatabase.IsValidFolder ("Assets/Tools/RubicalMe/ConverterPresets")) {
					AssetDatabase.CreateFolder ("Assets/Tools/RubicalMe", "ConverterPresets");
				}
				List<string> presets = new List<string> (Directory.GetFiles (Application.dataPath + "/Tools/RubicalMe/ConverterPresets"));
				int removalLength = Application.dataPath.Length + "/Tools/RubicalMe/ConverterPresets/".Length;
				int iOffset = 0;
				for (int i = 0; i < presets.Count; i++) {
					if (presets [i].Substring (presets [i].Length - 4, 4) == "meta") {
						presets.RemoveAt (i - iOffset);
						iOffset++;
						continue;
					}
					presets [i - iOffset] = presets [i - iOffset].Substring (removalLength, presets [i - iOffset].Length - removalLength - 6);
				}

				return presets;
			}

			public static TextureConverterPreset LoadPreset (string preset)
			{
				Debug.Log ("loading");
				return (TextureConverterPreset)AssetDatabase.LoadAssetAtPath ("Assets/Tools/RubicalMe/ConverterPresets/" + preset + ".asset", typeof(TextureConverterPreset));
			}

			private static T CreateOrReplaceAsset<T> (T asset, string path) where T:UnityEngine.Object
			{
				T existingAsset = AssetDatabase.LoadAssetAtPath<T> (path);

				if (existingAsset == null) {
					AssetDatabase.CreateAsset (asset, path);
					existingAsset = asset;
				} else {
					EditorUtility.CopySerialized (asset, existingAsset);
				}

				return existingAsset;
			}
		}
	}
}