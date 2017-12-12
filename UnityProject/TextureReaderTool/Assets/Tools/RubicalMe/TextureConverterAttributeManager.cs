#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class TextureConverterAttributeManager
{
	public static List<MethodInfo> conversionMethods;
	public static List<string> conversionMethodNames;

	[InitializeOnLoadMethod]
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void Manage ()
	{
		conversionMethods = new List<MethodInfo> ();
		System.Type[] types = Assembly.GetExecutingAssembly ().GetTypes ();
		foreach (var t in types) {
			foreach (var m in t.GetMethods (BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)) {
				TextureConversionMethod[] possibleMethods = (TextureConversionMethod[])m.GetCustomAttributes (typeof(TextureConversionMethod), false);
				if (possibleMethods.Length > 0) {
					conversionMethods.Add (m);
				}
			}
		}

		conversionMethodNames = new List<string> ();
		foreach (var m in conversionMethods) {
			conversionMethodNames.Add (m.ToString ());
		}
	}
}
#endif