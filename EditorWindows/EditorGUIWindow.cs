using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorGUIWindow : EditorWindow {

	[MenuItem("Editor Example/EditorGUI")]
	public static void Start () {
		Debug.Log("start");
		EditorGUIWindow window = EditorWindow.GetWindow<EditorGUIWindow>();
		window.Show();
	}

	Texture2D texture,invertedTexture;
	bool showInverted = false;
	void OnGUI()
	{
		texture = EditorGUILayout.ObjectField(
			"Add a Texture:",
			texture,
			typeof(Texture2D)) as Texture2D;
		if(GUI.Button(new Rect(8,25, position.width - 210, 20),"Process Inverted")) {
			if(invertedTexture)
				DestroyImmediate(invertedTexture);
			//Copy the new texture
			invertedTexture = new Texture2D(texture.width, 
				texture.height, 
				texture.format, 
				(texture.mipmapCount != 0));
			for (int m = 0; m < texture.mipmapCount; m++) 
				invertedTexture.SetPixels(texture.GetPixels(m), m);
			InvertColors();
			showInverted = true;
		}
		if(texture) {
			EditorGUI.LabelField(new Rect(25,45,100,15),new GUIContent("Preview:"));
			EditorGUI.DrawPreviewTexture(new Rect(25,60,100,100),texture);
			EditorGUI.DropShadowLabel(new Rect(150,45,100,15),new GUIContent("Alpha:"));
			EditorGUI.DrawTextureAlpha(new Rect(150,60,100,100),texture);
			EditorGUI.PrefixLabel(new Rect(275,45,100,15),0,new GUIContent("Inverted:"));
			if(showInverted)
				EditorGUI.DrawPreviewTexture(new Rect(275,60,100,100),invertedTexture);
			if(GUI.Button(new Rect(3,position.height - 25, position.width-6,20),"Clear texture")) {
				texture = EditorGUIUtility.whiteTexture;
				showInverted = false;
			}
		} else {
			EditorGUI.PrefixLabel(
				new Rect(3,position.height - 25,position.width - 6, 20),
				0,
				new GUIContent("No texture found"));
		}
	}

	void InvertColors()
	{
		for (int m = 0; m < invertedTexture.mipmapCount; m++) {
			Color[] c= invertedTexture.GetPixels(m);
			for (int i = 0 ;i < c.Length; i++) {
				c[i].r = 1 - c[i].r;
				c[i].g = 1 - c[i].g;
				c[i].b = 1 - c[i].b;
			}
			invertedTexture.SetPixels(c, m); 
		}
		invertedTexture.Apply();
	}

	void OnInspectorUpdate()
	{
		this.Repaint();
	}
}
