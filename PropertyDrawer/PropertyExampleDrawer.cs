using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(BigHeightAttribute))]
public class PropertyExaplerDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Simple CustomPropertyDrawer Example
//		EditorGUI.BeginProperty(position, label, property);
//		position.width = 200;
//		EditorGUI.PropertyField(position, property.FindPropertyRelative("position"), GUIContent.none);
//
//		position.x += 200;
//		EditorGUI.PropertyField(position, property.FindPropertyRelative("type"), GUIContent.none);
//		EditorGUI.EndProperty();
		EditorGUI.LabelField(position, label);
		position.y += EditorGUI.GetPropertyHeight(property);
		position.height = EditorGUI.GetPropertyHeight(property)* 2;
		EditorGUI.PropertyField(position, property, GUIContent.none);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label) * 3;
	}
}

[CustomPropertyDrawer(typeof(EnumLabelAttribute))]
public class EnumLabelDrawer : PropertyDrawer
{
	List<string> _names = new List<string>();
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		SetEnumName(property);
		property.enumValueIndex = EditorGUI.Popup(position,((EnumLabelAttribute)attribute).displayName, property.enumValueIndex, _names.ToArray());
	}

	private void SetEnumName(SerializedProperty property)
	{
		for (int idx = 0; idx < property.enumNames.Length; idx++)
		{
			var field = fieldInfo.FieldType.GetField(property.enumNames[idx]);
			var attrs = field.GetCustomAttributes(typeof(EnumLabelAttribute),true) as EnumLabelAttribute[];
			_names.Add(attrs[0].displayName);
		}
	}
}
