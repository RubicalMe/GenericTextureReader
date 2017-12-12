using System;

namespace RubicalMe
{
	namespace TextureConverterTools
	{
		public class TextureConverterException: Exception
		{
			public TextureConverterException ()
			{
			}

			public TextureConverterException (string message)
				: base (message)
			{
			}

			public TextureConverterException (string message, Exception inner)
				: base (message, inner)
			{
			}
		}
	}
}