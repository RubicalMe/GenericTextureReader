using UnityEngine;
using UnityEditor;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		public static class EditorGUILayoutExtra
		{
			public static char CharField (char curValue, int width)
			{
				string str = EditorGUILayout.TextField (curValue.ToString (), GUILayout.Width (width));
				if (str.Length > 0) {
					return str [0];
				} else {
					return ' ';
				}
			}

			public static Vector2 CleanVector2View (Vector2 curValue, int width)
			{
				EditorGUILayout.BeginHorizontal ();
				curValue.x = EditorGUILayout.FloatField (curValue.x, GUILayout.Width (width / 2));
				curValue.y = EditorGUILayout.FloatField (curValue.y, GUILayout.Width (width / 2));
				EditorGUILayout.EndHorizontal ();
				return curValue;
			}

			public static Vector3 CleanVector3View (Vector3 curValue, int width)
			{
				EditorGUILayout.BeginHorizontal ();
				curValue.x = EditorGUILayout.FloatField (curValue.x, GUILayout.Width (width / 3));
				curValue.y = EditorGUILayout.FloatField (curValue.y, GUILayout.Width (width / 3));
				curValue.z = EditorGUILayout.FloatField (curValue.z, GUILayout.Width (width / 3));
				EditorGUILayout.EndHorizontal ();
				return curValue;
			}
		}
	}
}