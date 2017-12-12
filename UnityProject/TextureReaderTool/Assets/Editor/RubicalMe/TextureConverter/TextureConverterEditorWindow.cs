using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RubicalMe;
using RubicalMe.TextureConverterTools;
using System.Reflection;
using System;
using System.IO;

public class TextureConverterEditorWindow : EditorWindow
{
	public enum PositionOrValue
	{
		UseValue,
		UseReadPosition
	}

	private Texture2D texture;
	private Color[] textureColors;
	private TextureConverterPreset currentPreset;
	private List<string> availablePresets;

	SerializedObject serializedpreset;

	[MenuItem ("Tools/RubicalMe/Texture Converter")]
	private static void Init ()
	{
		TextureConverterEditorWindow window = (TextureConverterEditorWindow)EditorWindow.GetWindow (typeof(TextureConverterEditorWindow));
		window.Show ();
	}

	private void OnEnable ()
	{
//		Debug.Log ("OnEnableCalled");
		availablePresets = PresetFileLoader.LoadPresets ();
		if (availablePresets.Count == 0) {
//			Debug.Log ("creating");
			TextureConverterPreset tempPreset = ScriptableObject.CreateInstance <TextureConverterPreset> ();
			Debug.Log (tempPreset.loadName);
			if (PresetFileLoader.SavePreset (tempPreset)) {
				availablePresets = PresetFileLoader.LoadPresets ();
			}
		}
		currentPreset = PresetFileLoader.LoadPreset (availablePresets [0]);
//		Debug.Log (availablePresets [0]);

		Undo.undoRedoPerformed += OnUndoRedo;
	}

	private void OnDisable ()
	{
		PresetFileLoader.SavePreset (currentPreset);
		Undo.undoRedoPerformed -= OnUndoRedo;
	}

	private void OnGUI ()
	{
		DisplayPresetPicker ();

		DisplayTopBar ();
		if (currentPreset.textureColors != null) {
			DisplayPreset ();
		}

		DisplayTexturePicker ();
		DisplayColors ();

		GUILayout.FlexibleSpace ();
		if (texture == null) {
			GUI.enabled = false;
		}
		if (GUILayout.Button ("execute")) {
			currentPreset.Execute (texture);
		}
		GUI.enabled = true;
	}

	private void OnTextureChange ()
	{
		if (texture == null) {
			textureColors = new Color[0];
			return;
		}
		textureColors = texture.Colors ();
	}

	private void DisplayPresetPicker ()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Current preset", EditorStyles.boldLabel, GUILayout.Width (150));
		int index = EditorGUILayout.Popup (availablePresets.IndexOf (currentPreset.loadName), availablePresets.ToArray ());
		if (index != availablePresets.IndexOf (currentPreset.loadName)) {
			if (PresetFileLoader.SavePreset (currentPreset)) {
				currentPreset = PresetFileLoader.LoadPreset (availablePresets [index]);
			}
		}
		if (GUILayout.Button ("NEW", GUILayout.Width (38))) {
			CreateNewPreset ();
		}
		GUILayout.EndHorizontal ();
		EditorGUI.BeginChangeCheck ();
		currentPreset.presetName = GUILayout.TextField (currentPreset.presetName);
		if (EditorGUI.EndChangeCheck () && currentPreset.presetName.Length > 0) {
			if (PresetFileLoader.SavePreset (currentPreset)) {
				availablePresets = PresetFileLoader.LoadPresets ();
			}
		}
	}

	private void CreateNewPreset ()
	{
		TextureConverterPreset tempPreset = ScriptableObject.CreateInstance <TextureConverterPreset> ();
		//	Debug.Log (tempPreset.loadName);
		while (availablePresets.Contains (tempPreset.loadName)) {
			tempPreset.loadName += "I";
			tempPreset.presetName += "I";
		}
		PresetFileLoader.SavePreset (tempPreset);
		string newPreset = tempPreset.loadName;
		availablePresets = PresetFileLoader.LoadPresets ();
		currentPreset = PresetFileLoader.LoadPreset (newPreset);
	}

	private void DisplayTexturePicker ()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Current loaded Texture", EditorStyles.boldLabel, GUILayout.Width (150));
		EditorGUI.BeginChangeCheck ();
		texture = EditorGUILayout.ObjectField (texture, typeof(Texture2D), true) as Texture2D;
		if (EditorGUI.EndChangeCheck ()) {
			OnTextureChange ();
		}
		GUILayout.EndHorizontal ();
	}

	private void DisplayColors ()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Colors in texture: ");
		GUILayout.Label ("(click to add to preset)", EditorStyles.centeredGreyMiniLabel);
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal (GUILayout.Height (40));
		GUILayout.Label ("", GUILayout.Width (10));
		if (textureColors != null) {
			foreach (Color c in textureColors) {
				Color oldColor = GUI.color;
				GUI.color = c;
				if (GUILayout.Button ("", GUI.skin.box, GUILayout.Height (11), GUILayout.Width (25))) {
					currentPreset.AddColor (c);
					Repaint ();
				}
				GUI.color = oldColor;
			}
		}
		GUILayout.EndHorizontal ();
	}

	private void DisplayPreset ()
	{
		for (int i = 0; i < currentPreset.textureColors.Count; i++) {
			DisplayPresetColor (currentPreset.textureColors [i], i);
		}
	}

	void DisplayPresetColor (Color color, int pos)
	{
		EditorGUILayout.BeginHorizontal (GUILayout.Height (22));
		GUILayout.Label ("", GUILayout.Width (80));
		currentPreset.useColor [pos] = EditorGUILayout.Toggle (currentPreset.useColor [pos], GUILayout.Width (15));
		GUILayout.Label ("", GUILayout.Width (1));
		DrawBoxWithColor (color);
		GUILayout.Label ("", GUILayout.Width (1));
		EditorGUI.BeginChangeCheck ();
		currentPreset.chosenMethods [pos] = EditorGUILayout.Popup (currentPreset.chosenMethods [pos], TextureConverterAttributeManager.conversionMethodNames.ToArray (), GUILayout.Width (300));
		if (EditorGUI.EndChangeCheck ()) {
			currentPreset.LoadMethodParameters (pos);
		}
		GUILayout.Label ("paramnum: " + currentPreset.chosenMethodsParameters [pos].Length);
		DrawParameters (pos);
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();
	}

	private void DisplayTopBar ()
	{
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("", EditorStyles.boldLabel, GUILayout.Width (80));
		GUILayout.Label ("use", EditorStyles.boldLabel, GUILayout.Width (25));
		GUILayout.Label ("", EditorStyles.boldLabel, GUILayout.Width (31));
		GUILayout.Label ("Invoke Method", EditorStyles.boldLabel);
		EditorGUILayout.EndHorizontal ();
	}

	private void DrawBoxWithColor (Color color)
	{
		Color oldColor = GUI.color;
		GUI.color = color;
		GUILayout.Box ("", GUILayout.Height (11), GUILayout.Width (25));
		GUI.color = oldColor;
	}

	private void DrawParameters (int method)
	{
		ParameterInfo[] mParameters = currentPreset.conversionMethods [currentPreset.chosenMethods [method]].GetParameters ();
		int paramLength = mParameters.Length;
		foreach (var item in mParameters) {
			if (item.ParameterType == typeof(UnityEngine.Vector2)) {
				paramLength++;
			}
		}
		if (paramLength != currentPreset.chosenMethodsParameters [method].Length) {
			TextureConverter.LoadMethodParameters (currentPreset, method);
		}
		int iOffset = 0;
		for (int i = 0; i < mParameters.Length; i++) {
			if (mParameters [i].ParameterType == typeof(UnityEngine.Vector2)) {
				Undo.RecordObject (currentPreset, "TextureConvereter Preset Change");
				currentPreset.chosenMethodsParameters [method] [i + iOffset] = TextureConverterDrawUtility.DrawParameterInput (typeof(PositionOrValue), currentPreset.chosenMethodsParameters [method] [i + iOffset]);
				if ((PositionOrValue)currentPreset.chosenMethodsParameters [method] [i + iOffset] == PositionOrValue.UseValue) {
					Undo.RecordObject (currentPreset, "TextureConvereter Preset Change");
					currentPreset.chosenMethodsParameters [method] [i + iOffset + 1] = TextureConverterDrawUtility.DrawParameterInput (mParameters [i].ParameterType, currentPreset.chosenMethodsParameters [method] [i + iOffset + 1]);
				}
				iOffset++;
			} else {
				Undo.RecordObject (currentPreset, "TextureConvereter Preset Change");
				currentPreset.chosenMethodsParameters [method] [i + iOffset] = TextureConverterDrawUtility.DrawParameterInput (mParameters [i].ParameterType, currentPreset.chosenMethodsParameters [method] [i + iOffset]);
			}
		}
	}

	private void OnUndoRedo ()
	{
		Repaint ();
	}
}
