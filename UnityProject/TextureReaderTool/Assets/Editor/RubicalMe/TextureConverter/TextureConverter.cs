using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;
using RubicalMe.TextureConverterTools;
using System.Reflection;
using System;
using System.IO;

public static class TextureConverter
{
	public static void RedefineConversionMethodsInPreset (this TextureConverterPreset currentPreset, List<MethodInfo> conversionMethods)
	{
		Debug.Log ("Conversion Method definitions changed, redefine called on preset " + currentPreset.presetName);

		//		Debug.Log (currentPreset.conversionMethods.Count);
		if (currentPreset.conversionMethods == null || currentPreset.chosenMethods == null) {
//			Debug.Log ("immediate conversion because null");
			currentPreset.conversionMethods = conversionMethods;
			return;
		}
		for (int iMethod = 0; iMethod < currentPreset.conversionMethods.Count; iMethod++) {
			if (!conversionMethods.Contains (currentPreset.conversionMethods [iMethod])) {
				Debug.LogWarning ("Method " + currentPreset.conversionMethods [iMethod] + " does not exist anymore and has been removed from this preset");
				for (int i = 0; i < currentPreset.chosenMethods.Count; i++) {
					if (currentPreset.chosenMethods [i] == iMethod) {
						currentPreset.chosenMethods [i] = -1;
					}
				}
			}
		}
		for (int i = 0; i < currentPreset.chosenMethods.Count; i++) {
			if (currentPreset.chosenMethods [i] == -1) {
				currentPreset.chosenMethods [i] = 0;
				LoadMethodParameters (currentPreset, i);
			} else {
				currentPreset.chosenMethods [i] = conversionMethods.IndexOf (currentPreset.conversionMethods [currentPreset.chosenMethods [i]]);
			}
		}
		currentPreset.conversionMethods = conversionMethods;
	}

	public static void LoadMethodParameters (this TextureConverterPreset currentPreset, int method)
	{
//		Debug.Log (currentPreset.conversionMethods);
//		Debug.Log (TextureConverterAttributeManager.conversionMethods.Count);
		ParameterInfo[] mParameters = currentPreset.conversionMethods [currentPreset.chosenMethods [method]].GetParameters ();
		int paramLength = mParameters.Length;
		foreach (var item in mParameters) {
			if (item.ParameterType == typeof(UnityEngine.Vector2)) {
				paramLength++;
			}
		}
		currentPreset.chosenMethodsParameters [method] = new object[paramLength];
	}

	public static void Execute (this TextureConverterPreset currentPreset, Texture2D texture)
	{
		List<Color> matchList = new List<Color> (currentPreset.textureColors);

		for (int x = 0; x < texture.width; x++) {
			for (int y = 0; y < texture.height; y++) {
				if (matchList.Contains (texture.GetPixel (x, y))) {
					int index = matchList.IndexOf (texture.GetPixel (x, y));
					ExecuteIndex (currentPreset, index, new Vector2 (x, y));
				}
			}
		}
	}

	private static void ExecuteIndex (TextureConverterPreset currentPreset, int i, Vector2 pos)
	{
		if (!currentPreset.useColor [i]) {
			return;
		}

		ParameterInfo[] mParameters = currentPreset.conversionMethods [currentPreset.chosenMethods [i]].GetParameters ();
		int iOffset = 0;
		object[] parameters = new object[mParameters.Length];
		for (int iParams = 0; iParams < mParameters.Length; iParams++) {
			if (mParameters [iParams].ParameterType == typeof(UnityEngine.Vector2)) {
				if ((TextureConverterEditorWindow.PositionOrValue)currentPreset.chosenMethodsParameters [i] [iParams + iOffset] == TextureConverterEditorWindow.PositionOrValue.UseValue) {
					//	Debug.Log (parameters [i]);
					parameters [iParams] = currentPreset.chosenMethodsParameters [i] [iParams + iOffset + 1];
				} else {
					parameters [iParams] = pos;
				}
				iOffset++;
			} else {
				parameters [iParams] = currentPreset.chosenMethodsParameters [i] [iParams + iOffset];
			}
		}

		currentPreset.conversionMethods [currentPreset.chosenMethods [i]].Invoke (null, parameters);
	}
}
