using UnityEditor;
using UnityEngine;
using System;

namespace RubicalMe
{
	public static class TextureConverterDrawUtility
	{
		public static object DrawParameterInput (Type parameterType, object parameter)
		{
			int defaultWidth = 100;
			if (parameterType == typeof(bool)) {
				if (parameter == null) {
					return EditorGUILayout.Toggle (false);
				} else {
					return EditorGUILayout.Toggle ((bool)parameter);
				}
			} else if (parameterType == typeof(char)) {
				if (parameter == null) {
					return EditorGUILayoutExtra.CharField (' ', defaultWidth);
				} else {
					return EditorGUILayoutExtra.CharField ((char)parameter, defaultWidth);
				}
			} else if (parameterType == typeof(int)) {
				if (parameter == null) {
					return EditorGUILayout.IntField (0, GUILayout.Width (defaultWidth));
				} else {
					return EditorGUILayout.IntField ((int)parameter, GUILayout.Width (defaultWidth));
				}
			} else if (parameterType == typeof(double)) {
				if (parameter == null) {
					return EditorGUILayout.DoubleField (0, GUILayout.Width (defaultWidth));
				} else {
					return EditorGUILayout.DoubleField ((double)parameter, GUILayout.Width (defaultWidth));
				}
			} else if (parameterType == typeof(float)) {
				if (parameter == null) {
					return EditorGUILayout.FloatField (0, GUILayout.Width (defaultWidth));
				} else {
					return EditorGUILayout.FloatField ((float)parameter, GUILayout.Width (defaultWidth));
				}
			} else if (parameterType == typeof(long)) {
				if (parameter == null) {
					return EditorGUILayout.LongField (0, GUILayout.Width (defaultWidth));
				} else {
					return EditorGUILayout.LongField ((long)parameter, GUILayout.Width (defaultWidth));
				}
			} else if (parameterType == typeof(string)) {
				if (parameter == null) {
					return EditorGUILayout.TextField ("", GUILayout.Width (defaultWidth));
				} else {
					return EditorGUILayout.TextField ((string)parameter, GUILayout.Width (defaultWidth));
				}
			} else if (parameterType == typeof(UnityEngine.Vector2)) {
				if (parameter == null) {
					return EditorGUILayoutExtra.CleanVector2View (Vector2.zero, defaultWidth);
				} else {
					return EditorGUILayoutExtra.CleanVector2View ((Vector2)parameter, defaultWidth);
				}
			} else if (parameterType == typeof(UnityEngine.Vector3)) {
				if (parameter == null) {
					return EditorGUILayoutExtra.CleanVector3View (Vector3.zero, defaultWidth);
				} else {
					return EditorGUILayoutExtra.CleanVector3View ((Vector3)parameter, defaultWidth);
				}
			} else if (parameterType == typeof(TextureConverterEditorWindow.PositionOrValue)) {
				if (parameter == null) {
					return EditorGUILayout.EnumPopup (TextureConverterEditorWindow.PositionOrValue.UseReadPosition, GUILayout.Width (defaultWidth));
				} else {
					return EditorGUILayout.EnumPopup ((TextureConverterEditorWindow.PositionOrValue)parameter, GUILayout.Width (defaultWidth));
				}
			} else {
				try {
					return EditorGUILayout.ObjectField ((UnityEngine.Object)parameter, parameterType, false, GUILayout.Width (defaultWidth));
				} catch (UnityEngine.ExitGUIException e) {
					throw e;
				} catch (Exception e) {
					Debug.LogWarning (e.Message);
					throw new System.ArgumentException ("Parameterinput has not been implemented for the type \"" + parameterType.FullName + "\" and the default UnityEngine.Object typecast could not be used instead.");
				}
			}
		}
	}
}