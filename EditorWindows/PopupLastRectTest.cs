using UnityEngine;
using UnityEditor;

public class EditorWindowWithPopup : EditorWindow
{

	// Add menu item
	[MenuItem("Editor Example/Popup Example")]
	static void Init()
	{
		var window = EditorWindow.CreateInstance<EditorWindowWithPopup>();
		window.Show();
	}

	Rect buttonRect;
	void OnGUI()
	{
		{
			GUILayout.Label("Editor window with Popup example",EditorStyles.boldLabel);
			if (GUILayout.Button("Popup Options",GUILayout.Width(200)))
			{
				PopupWindow.Show(buttonRect, new PopupExample());
			}
			if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
		}
	}
}

public class PopupExample : PopupWindowContent
{
	bool toggle1 = true;
	bool toggle2 = true;
	bool toggle3 = true;

	public override Vector2 GetWindowSize()
	{
		return new Vector2(200,150);
	}

	public override void OnGUI(Rect rect)
	{
		GUILayout.Label("Popup Options Example", EditorStyles.boldLabel);
		toggle1 = EditorGUILayout.Toggle("Toggle 1", toggle1);
		toggle2 = EditorGUILayout.Toggle("Toggle 2", toggle2);
		toggle3 = EditorGUILayout.Toggle("Toggle 3", toggle3);
	}

	public override void OnOpen()
	{
		Debug.Log("Popup opened: " + this);
	}

	public override void OnClose()
	{
		Debug.Log("Popup closed: " + this);
	}
}