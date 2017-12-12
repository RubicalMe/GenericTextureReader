using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureConverterClass : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	[TextureConversionMethod]
	public static void SomeMethodWithPos (Vector2 position)
	{
		Debug.Log ("activated " + position);
	}

	[TextureConversionMethod]
	public static void SomeMethodWithChar (char c)
	{
		Debug.Log ("activated " + c);
	}

	[TextureConversionMethod]
	public static void SomeMethodWithChar (char c, Vector2 v)
	{
		Debug.Log ("activated " + c + v);
	}

	[TextureConversionMethod]
	public static void SomeMethodWithTexture (Texture2D t)
	{
		Debug.Log ("activated " + t.name);
	}

	[TextureConversionMethod]
	public static void SomeMethodWithoutPos ()
	{
		Debug.Log ("activated");
	}

	[TextureConversionMethod]
	public static void SomeMethodWithGameObject (GameObject obj, Vector2 pos)
	{
		Debug.Log ("activated " + pos + " | " + obj.name);
	}
}
