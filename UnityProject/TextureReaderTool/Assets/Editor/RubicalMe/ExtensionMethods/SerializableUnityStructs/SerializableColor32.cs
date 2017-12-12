using System;
using UnityEngine;

namespace RubicalMe
{
	namespace EditorExtensions
	{
		[Serializable]
		public struct SerializableColor32
		{
			public byte r;
			public byte g;
			public byte b;
			public byte a;

			public SerializableColor32 (byte r, byte g, byte b, byte a)
			{
				this.r = r;
				this.g = g;
				this.b = b;
				this.a = a;
			}

			public static implicit operator SerializableColor32 (Color32 c)
			{
				return new SerializableColor32 (c.r, c.g, c.b, c.a);
			}

			public static implicit operator Color32 (SerializableColor32 sc)
			{
				return new Color32 (sc.r, sc.g, sc.b, sc.a);
			}
		}
	}
}