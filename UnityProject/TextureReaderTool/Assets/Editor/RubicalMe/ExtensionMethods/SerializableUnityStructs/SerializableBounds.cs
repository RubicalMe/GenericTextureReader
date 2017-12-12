using System;
using UnityEngine;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		[Serializable]
		public struct SerializableBounds
		{
			public SerializableVector3 center;
			public SerializableVector3 size;

			public SerializableBounds (SerializableVector3 center, SerializableVector3 size)
			{
				this.center = center;
				this.size = size;
			}

			public static implicit operator SerializableBounds (Bounds b)
			{
				return new SerializableBounds (b.center, b.size);
			}

			public static implicit operator Bounds (SerializableBounds b)
			{
				return new Bounds (b.center, b.size);
			}
		}
	}
}