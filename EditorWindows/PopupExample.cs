using UnityEngine;
using UnityEditor;

public class ShowPopupExample : EditorWindow
{
	public static ShowPopupExample window;
	public static void Init(Rect? rect = null)
	{
		window = CreateInstance<ShowPopupExample>();
		window.position = rect != null ? new Rect(rect.Value.x, rect.Value.y, 250, 350): new Rect(200, 300, 250, 350);
		window.ShowPopup();
	}

	void OnGUI()
	{
		EditorGUILayout.LabelField("This is an example of EditorWindow.ShowPopup",EditorStyles.wordWrappedLabel);
		GUILayout.Space(70);
		if (GUILayout.Button("Agree!")) this.Close();
	}

	public static void OnClose()
	{
		window.Close();
	}
}

public class PopupContentExample : PopupWindowContent
{
	PopupContentExample example;
	public override Vector2 GetWindowSize()
	{
		return new Vector2(300,150);
	}

	public override void OnOpen()
	{
		Debug.Assert(!editorWindow.Equals(null));
		editorWindow.titleContent = new GUIContent("Popup Open");
	}

	public override void OnClose()
	{
		editorWindow.titleContent = new GUIContent("Close");

	}
	float fVal;
	public override void OnGUI(Rect rect)
	{
		EditorGUILayout.LabelField("This is an example of PopupWindow.ShowPopup",EditorStyles.wordWrappedLabel);
		GUILayout.Space(70);
		fVal = EditorGUILayout.Slider(fVal,100f,1000f);
		if (GUILayout.Button("Change!")) 
		{
			editorWindow.position = new Rect(fVal, Screen.height / 2, 250, 150);
		}
	}
}