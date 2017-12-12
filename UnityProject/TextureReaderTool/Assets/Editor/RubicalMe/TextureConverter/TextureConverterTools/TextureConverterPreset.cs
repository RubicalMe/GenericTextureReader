using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace RubicalMe
{
	namespace TextureConverterTools
	{
		public class TextureConverterPreset : ScriptableObject
		{
			public List<Color> textureColors;
			public List<int> chosenMethods;
			public List<bool> useColor;
			// TODO: object[] list cannot be serialized and is not saved on preset save... find other way to save
			public List<object[]> chosenMethodsParameters;
			public List<MethodInfo> conversionMethods;
			public string presetName;
			public string loadName;

			public TextureConverterPreset ()
			{
				loadName = "newPreset";
				presetName = "newPreset";
				textureColors = new List<Color> ();
				useColor = new List<bool> ();
				chosenMethods = new List<int> ();
				chosenMethodsParameters = new List<object[]> ();
			}

			public TextureConverterPreset (string name)
			{
				loadName = name;
				presetName = name;
			}

			public void AddColor (Color color)
			{
				if (textureColors.Contains (color)) {
					return;
				}

				textureColors.Add (color);
				chosenMethods.Add (0);
				useColor.Add (false);
				if (chosenMethodsParameters == null) {
					chosenMethodsParameters = new List<object[]> ();
				}
				chosenMethodsParameters.Add (new object[0]);
				this.LoadMethodParameters (chosenMethodsParameters.Count - 1);
			}
		}
	}
}