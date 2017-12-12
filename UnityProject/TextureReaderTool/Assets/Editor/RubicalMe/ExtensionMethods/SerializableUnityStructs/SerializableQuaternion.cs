using System;
using UnityEngine;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		[Serializable]
		public struct SerializableQuaternion
		{
			public float w;
			public float x;
			public float y;
			public float z;

			public SerializableQuaternion (float w, float x, float y, float z)
			{
				this.w = w;
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public static implicit operator SerializableQuaternion (Quaternion q)
			{
				return new SerializableQuaternion (q.w, q.x, q.y, q.z);
			}

			public static implicit operator Quaternion (SerializableQuaternion q)
			{
				return new Quaternion (q.w, q.x, q.y, q.z);
			}
		}
	}
}