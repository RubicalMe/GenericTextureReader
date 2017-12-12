using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubicalMe
{
	namespace TextureConverterTools
	{
		public static class TextureAnalyser
		{
			public static Color[] Colors (this Texture2D tex)
			{
				if (tex == null) {
					throw new System.NullReferenceException ();
				}
				if (tex.width == 0 || tex.height == 0) {
					throw new System.ArgumentException ("Cannot read colors of texture because there are no pixels");
				}
				try {
					tex.GetPixel (0, 0);
				} catch (UnityException) {
					throw new TextureConverterException ("Cannot read colors of texture. Did you set the texture readable in the Texture Import Settings?");
				}
				List<Color> colorsFound = new List<Color> ();
				for (int x = 0; x < tex.width; x++) {
					for (int y = 0; y < tex.height; y++) {
						Color c = tex.GetPixel (x, y);
						if (!colorsFound.Contains (c)) {
							colorsFound.Add (c);
						}
					}
				}
				//	colorsFound.Sort ();
				return colorsFound.ToArray ();
			}
		}
	}
}