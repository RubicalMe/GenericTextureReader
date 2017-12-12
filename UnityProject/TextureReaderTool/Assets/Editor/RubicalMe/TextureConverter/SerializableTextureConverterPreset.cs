using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

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

		[Serializable]
		public class SerializableColor
		{
			public float r;
			public float g;
			public float b;
			public float a;

			public SerializableColor ()
			{
				r = 1;
				g = 1;
				b = 1;
				a = 1;
			}

			public SerializableColor (float r, float g, float b, float a)
			{
				this.r = r;
				this.g = g;
				this.b = b;
				this.a = a;
			}

			public static implicit operator SerializableColor (Color c)
			{
				return new SerializableColor (c.r, c.g, c.b, c.a);
			}

			public static implicit operator Color (SerializableColor sc)
			{
				return new Color (sc.r, sc.g, sc.b, sc.a);
			}
		}
	}
}
