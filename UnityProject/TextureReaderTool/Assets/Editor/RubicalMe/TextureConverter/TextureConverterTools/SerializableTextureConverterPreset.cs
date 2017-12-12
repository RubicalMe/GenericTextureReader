using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using RubicalMe.EditorExtensions;

namespace RubicalMe
{
	namespace TextureConverterTools
	{
		[Serializable]
		public class SerializableTextureConverterPreset
		{
			public List<SerializableColor> textureColors;
			public List<int> chosenMethods;
			public List<bool> useColor;
			// TODO: object[] list cannot be serialized and is not saved on preset save... find other way to save
			public List<object[]> chosenMethodsParameters;
			public List<MethodInfo> conversionMethods;
			public string presetName;
			public string loadName;

			public static explicit operator SerializableTextureConverterPreset (TextureConverterPreset preset)
			{
				SerializableTextureConverterPreset tempSTCP = new SerializableTextureConverterPreset ();
				tempSTCP.textureColors = new List<SerializableColor> ();
				foreach (var item in preset.textureColors) {
					tempSTCP.textureColors.Add (item);
				}

				for (int im = 0; im < preset.chosenMethodsParameters.Count; im++) {
					for (int i = 0; i < preset.chosenMethodsParameters [im].Length; i++) {
						if (preset.chosenMethodsParameters [im] [i] is UnityEngine.Object) {
							preset.chosenMethodsParameters [im] [i] = "#obj#" + AssetDatabase.AssetPathToGUID (AssetDatabase.GetAssetPath ((UnityEngine.Object)preset.chosenMethodsParameters [im] [i]));
						} else {
							if (preset.chosenMethodsParameters [im] [i] != null) {
								Type t = preset.chosenMethodsParameters [im] [i].GetType ();
								if (t.IsValueType) {
									if (t.Namespace == "UnityEngine") {
										preset.chosenMethodsParameters [im] [i] = SerializableUnityStructs.ConvertToSerializableStruct (preset.chosenMethodsParameters [im] [i]);
									}
								}
							}
						}
					}
				}

				tempSTCP.chosenMethods = preset.chosenMethods;
				tempSTCP.useColor = preset.useColor;
				tempSTCP.chosenMethods = preset.chosenMethods;
				tempSTCP.chosenMethodsParameters = preset.chosenMethodsParameters;
				tempSTCP.conversionMethods = preset.conversionMethods;
				tempSTCP.presetName = preset.presetName;
				tempSTCP.loadName = preset.loadName;
				return tempSTCP;
			}

			public static explicit operator TextureConverterPreset (SerializableTextureConverterPreset preset)
			{
				TextureConverterPreset tempSTCP = ScriptableObject.CreateInstance <TextureConverterPreset> ();
				tempSTCP.textureColors = new List<Color> ();
				foreach (var item in preset.textureColors) {
					tempSTCP.textureColors.Add (item);
				}

				for (int im = 0; im < preset.chosenMethodsParameters.Count; im++) {
					for (int i = 0; i < preset.chosenMethodsParameters [im].Length; i++) {
						if (preset.chosenMethodsParameters [im] [i] is string) {
							if (((string)preset.chosenMethodsParameters [im] [i]).StartsWith ("#obj#")) {
								preset.chosenMethodsParameters [im] [i] = AssetDatabase.LoadAssetAtPath (AssetDatabase.GUIDToAssetPath (((string)preset.chosenMethodsParameters [im] [i]).Substring (5, ((string)preset.chosenMethodsParameters [im] [i]).Length - 5)), typeof(UnityEngine.Object));
							}
						} else {
							if (preset.chosenMethodsParameters [im] [i] != null) {
								Type t = preset.chosenMethodsParameters [im] [i].GetType ();
								if (t.IsValueType) {
									if (t.Namespace == "RubicalMe.EditorExtensions") {
										preset.chosenMethodsParameters [im] [i] = SerializableUnityStructs.ConvertToUnityStruct (preset.chosenMethodsParameters [im] [i]);
									}
								}
							}
						}
					}
				}
				tempSTCP.chosenMethods = preset.chosenMethods;
				tempSTCP.useColor = preset.useColor;
				tempSTCP.chosenMethods = preset.chosenMethods;
				tempSTCP.chosenMethodsParameters = preset.chosenMethodsParameters;
				tempSTCP.conversionMethods = preset.conversionMethods;
				tempSTCP.presetName = preset.presetName;
				tempSTCP.loadName = preset.loadName;
				return tempSTCP;
			}
		}
	}
}
