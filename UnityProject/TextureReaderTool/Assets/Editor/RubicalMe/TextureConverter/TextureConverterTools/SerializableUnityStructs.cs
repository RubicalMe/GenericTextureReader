using System;
using UnityEngine;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		public static class SerializableUnityStructs
		{
			public static object ConvertToSerializableStruct (object o)
			{
				if (o is Vector2) {
					return (SerializableVector2)(Vector2)o;
				} else if (o is Vector3) {
					return (SerializableVector3)(Vector3)o;
				} else if (o is Vector4) {
					return (SerializableVector4)(Vector4)o;
				} else if (o is Quaternion) {
					return (SerializableQuaternion)(Quaternion)o;
				} else if (o is Rect) {
					return (SerializableRect)(Rect)o;
				} else if (o is Color) {
					return (SerializableColor)(Color)o;
				} else if (o is Color32) {
					return (SerializableColor32)(Color32)o;
				} else if (o is Bounds) {
					return (SerializableBounds)(Bounds)o;
				}
				throw new ArgumentException ("No conversion for " + o.GetType ().FullName);
			}

			public static object ConvertToUnityStruct (object o)
			{
				if (o is SerializableVector2) {
					return (Vector2)(SerializableVector2)o;
				} else if (o is SerializableVector3) {
					return (Vector3)(SerializableVector3)o;
				} else if (o is SerializableVector4) {
					return (Vector4)(SerializableVector4)o;
				} else if (o is SerializableQuaternion) {
					return (Quaternion)(SerializableQuaternion)o;
				} else if (o is SerializableRect) {
					return (Rect)(SerializableRect)o;
				} else if (o is SerializableColor) {
					return (Color)(SerializableColor)o;
				} else if (o is SerializableColor32) {
					return (Color32)(SerializableColor32)o;
				} else if (o is SerializableBounds) {
					return (Bounds)(SerializableBounds)o;
				}
				throw new ArgumentException ("No conversion for " + o.GetType ().FullName);
			}
		}
	}
}