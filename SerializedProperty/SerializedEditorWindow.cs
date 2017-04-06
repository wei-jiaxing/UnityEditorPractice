using UnityEngine;
using UnityEditor;
using System.Collections;

public class SerializedEditorWindow : EditorWindow {
	static SerializedObject so,so2,soArray;
	static SerializedObjectTest test;
	static SerializedObjectTest[] tests;
	[MenuItem("Editor Example/SerializeTest")]
	public static void Start () {
		SerializedEditorWindow window = EditorWindow.GetWindow<SerializedEditorWindow>();
		Init();
		window.Show();
	}

	private static void Init()
	{
		test = FindObjectOfType<SerializedObjectTest>();
		so = new SerializedObject(test);
		so2 = new SerializedObject(test);
		tests = FindObjectsOfType<SerializedObjectTest>();
		soArray = new SerializedObject(tests);
	}

	private int index;
	void OnGUI()
	{
		if (test == null)
			Init();
//		EditorGUILayout.LabelField(test.index+"");
		EditorGUILayout.PrefixLabel("so1:"+so.FindProperty("index").intValue+"");
		EditorGUI.indentLevel ++;
		EditorGUILayout.LabelField("so2:",so2.FindProperty("index").intValue+"");
		EditorGUILayout.SelectableLabel("soArray:"+soArray.FindProperty("index").intValue+"");
		EditorGUI.indentLevel --;
		index = EditorGUILayout.IntField("index",so.FindProperty("index").intValue);
		so.FindProperty("index").intValue = index;
		so.ApplyModifiedProperties();

		so2.UpdateIfDirtyOrScript();
		index = EditorGUILayout.IntField("index",so2.FindProperty("index").intValue);
		so2.FindProperty("index").intValue = index;
		so2.ApplyModifiedPropertiesWithoutUndo();

		soArray.UpdateIfDirtyOrScript();
		index = EditorGUILayout.IntField("index",soArray.FindProperty("index").intValue);
		soArray.FindProperty("index").intValue = index;
		soArray.ApplyModifiedProperties();
	}

	void OnInspectorUpdate()
	{
		this.Repaint();
	}
}
