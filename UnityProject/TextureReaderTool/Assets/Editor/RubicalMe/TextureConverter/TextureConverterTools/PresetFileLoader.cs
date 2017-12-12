using System.IO;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace RubicalMe
{
	namespace TextureConverterTools
	{
		public static class PresetFileLoader
		{
			private static string Path {
				get {
					return Application.dataPath + "/Tools/RubicalMe/ConverterPresets";
				}
			}

			private static string FileExtension {
				get {
					return ".preset";
				}
			}

			private static string SplitChar {
				get {
					return "<";
				}
			}

			public static bool SavePreset (TextureConverterPreset preset)
			{
//				Debug.Log ("saving");

				string oldPresetPath = Path + "/" + preset.loadName + FileExtension;
				if (File.Exists (oldPresetPath)) {
					File.Delete (oldPresetPath);
				}

				List<string> existingPresets = LoadPresets ();

				preset.loadName = preset.presetName;
				while (existingPresets.Contains (preset.loadName) || preset.loadName == "") {
					preset.loadName += "I";
					preset.presetName += "I";
				}

				for (int im = 0; im < preset.chosenMethodsParameters.Count; im++) {
					for (int i = 0; i < preset.chosenMethodsParameters [im].Length; i++) {
						if (preset.chosenMethodsParameters [im] [i] is UnityEngine.Object) {
							preset.chosenMethodsParameters [im] [i] = "#obj#" + AssetDatabase.AssetPathToGUID (AssetDatabase.GetAssetPath ((UnityEngine.Object)preset.chosenMethodsParameters [im] [i]));
						}
					}
				}

				Stream stream = File.Open (Path + "/" + preset.presetName + FileExtension, FileMode.Create);
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Serialize (stream, (SerializableTextureConverterPreset)preset);
				stream.Close ();
				formatter = null;

//				Debug.Log ("done saving");

				return true;
			}

			public static TextureConverterPreset LoadPreset (string name)
			{
//				Debug.Log ("loading \n" + name);

				string presetPath = Path + "/" + name + FileExtension;

				if (!File.Exists (presetPath)) {
					throw new System.ArgumentException ("There is no preset in the designated folder for " + name);
				}
					
				Stream stream = File.Open (Path + "/" + name + FileExtension, FileMode.Open);
				BinaryFormatter formatter = new BinaryFormatter ();
				SerializableTextureConverterPreset preset = (SerializableTextureConverterPreset)formatter.Deserialize (stream);
				stream.Close ();
				formatter = null;

				for (int im = 0; im < preset.chosenMethodsParameters.Count; im++) {
					for (int i = 0; i < preset.chosenMethodsParameters [im].Length; i++) {
						if (preset.chosenMethodsParameters [im] [i] is string) {
							if (((string)preset.chosenMethodsParameters [im] [i]).StartsWith ("#obj#")) {
								preset.chosenMethodsParameters [im] [i] = AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (((string)preset.chosenMethodsParameters [im] [i]).Substring (5, ((string)preset.chosenMethodsParameters [im] [i]).Length - 5)), typeof(UnityEngine.Object));
							}
						}
					}
				}


				List<MethodInfo> conversionMethods = TextureConverterAttributeManager.conversionMethods;

				TextureConverterPreset convertedPreset = (TextureConverterPreset)preset;

				if (!AreEqual (conversionMethods, convertedPreset.conversionMethods)) {
					convertedPreset.RedefineConversionMethodsInPreset (conversionMethods);
				}

//				Debug.Log ("done loading");

				return convertedPreset;
			}

			public static List<string> LoadPresets ()
			{
				List<string> paths = new List<string> (Directory.GetFiles (Path));
//				Debug.Log (PrintList (paths));
//				Debug.Log (paths.Count);
				int iOffset = 0;
				int pathCount = paths.Count;
				for (int i = 0; i < pathCount; i++) {
//					Debug.Log ("itterator ================ " + i);
					if (!paths [i - iOffset].EndsWith (FileExtension)) {
						paths.RemoveAt (i - iOffset);
						iOffset++;
//						Debug.Log ("should remove");
						continue;
					}

//					Debug.Log (paths [i - iOffset]);
//					Debug.Log (paths [i - iOffset].Length);
//					Debug.Log (paths [i - iOffset].Length - Path.Length - FileExtension.Length);
					paths [i - iOffset] = paths [i - iOffset].Substring (Path.Length + 1, paths [i - iOffset].Length - Path.Length - FileExtension.Length - 1);
				}
//				Debug.Log (PrintList (paths));
				return paths;
			}

			private static string PrintList (List<string> list)
			{
				string listString = "";
				foreach (var item in list) {
					listString += item + "\n";
				}
				return listString;
			}


			private static bool AreEqual <T> (List<T> lhs, List<T> rhs) // where T: object
			{
				if (lhs == null || rhs == null)
					return false;
				if (lhs.Count != rhs.Count) {
					return false;
				}
				for (int i = 0; i < lhs.Count; i++) {
					if (!Equals (lhs [i], rhs [i])) {
						return false;
					}
				}
				return true;
			}

		}
	}
}