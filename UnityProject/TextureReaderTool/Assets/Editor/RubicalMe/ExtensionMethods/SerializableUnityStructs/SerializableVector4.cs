using System;
using UnityEngine;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		[Serializable]
		public struct SerializableVector4
		{
			public float w;
			public float x;
			public float y;
			public float z;

			public SerializableVector4 (float w, float x, float y, float z)
			{
				this.w = w;
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public static implicit operator SerializableVector4 (Vector4 v)
			{
				return new SerializableVector4 (v.w, v.x, v.y, v.z);
			}

			public static implicit operator Vector4 (SerializableVector4 v)
			{
				return new Vector4 (v.w, v.x, v.y, v.z);
			}
		}
	}
}