using System;
using UnityEngine;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		[Serializable]
		public struct SerializableRect
		{
			public SerializableVector2 position;
			public SerializableVector2 size;

			public SerializableRect (SerializableVector2 position, SerializableVector2 size)
			{
				this.position = position;
				this.size = size;
			}

			public SerializableRect (float x, float y, float width, float height)
			{
				this.position = new SerializableVector2 (x, y);
				this.size = new SerializableVector2 (width, height);
			}


			public static implicit operator SerializableRect (Rect r)
			{
				return new SerializableRect (r.position, r.size);
			}

			public static implicit operator Rect (SerializableRect r)
			{
				return new Rect (r.position, r.size);
			}
		}
	}
}