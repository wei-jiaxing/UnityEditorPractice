using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sample))]
public class SampleDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Rect rect = EditorGUI.PrefixLabel(position, label);
		EditorGUI.indentLevel = 0;

		rect.x -= 30;
		rect.width = 60;
		EditorGUI.PropertyField(rect, property.FindPropertyRelative("name"), GUIContent.none);

		rect.x += 70;
		rect.width = 60;
		EditorGUI.PropertyField(rect, property.FindPropertyRelative("job"), GUIContent.none);

		rect.x += 70;
		EditorGUI.PropertyField(rect, property.FindPropertyRelative("level"), GUIContent.none);
	}
}
