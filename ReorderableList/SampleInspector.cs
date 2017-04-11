using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ReorderableListSample))]
public class SampleInspector : Editor
{
	ReorderableList list;
	int header;
	public void OnEnable()
	{
		var t = (ReorderableListSample)target;
		list = new ReorderableList(t.samples, typeof(ReorderableListSample));

		list.drawHeaderCallback = (rect) =>
		{
			EditorGUI.LabelField(rect, "Sample Header");
			rect.x += 100;
		};
		list.drawElementCallback = OnDrawElementCallback;

		list.onCanRemoveCallback = (ReorderableList l) =>
		{  
			return l.count > 1;
		};
			
		list.onAddCallback += (li) =>
		{
			var menu = new GenericMenu ();
			menu.AddItem(
				new GUIContent("Item1"),
				false,
				OnMenuClick,
				System.DateTime.Now.ToLongTimeString()
			);

			menu.AddSeparator("");

			int random = Random.Range(1,3);
			if (random == 1)
			{
				menu.AddItem(
					new GUIContent("Item2"),
					false,
					() => list.list.Insert(Random.Range(0, list.count), new Sample())
				);
			}
			else
			{
				menu.AddDisabledItem(new GUIContent("Item2"));
			}

			menu.ShowAsContext();
		};

	}

	private void OnMenuClick(object obj)
	{
		var sample = new Sample();
		sample.name = obj.ToString();
		sample.level = Random.Range(0, 100);
		sample.job = (Sample.Job)Random.Range(0, 3);
		list.list.Add(sample);
	}

	public override void OnInspectorGUI()
	{
//		base.OnInspectorGUI();
		SerializedObject so = new SerializedObject(target);
		EditorGUILayout.PropertyField(so.FindProperty("sample"));
		list.DoLayoutList();
	}

	private void OnDrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
	{
		var t = (ReorderableListSample)target;
		var element = t.samples[index];
		rect.y += 2;
		rect.height = EditorGUIUtility.singleLineHeight;

		if (isActive)
		{
			rect.width = 60;
			element.name = EditorGUI.TextField(rect, element.name);

			rect.x += 60;
			rect.width = 60;
			element.job = (Sample.Job)EditorGUI.EnumPopup(rect, element.job);

			rect.x += 60;
			rect.width = 60;
			element.level = EditorGUI.IntField(rect, element.level);

			rect.x += 60;
			rect.width = 20;
			GUI.enabled = false;
			isFocused = EditorGUI.Toggle(rect, isFocused);
			GUI.enabled = true;
		}
		else
		{
			rect.width = 60;
			EditorGUI.LabelField(rect, new GUIContent(element.name));
		}
	}
}
