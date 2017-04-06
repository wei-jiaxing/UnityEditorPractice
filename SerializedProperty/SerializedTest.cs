using UnityEngine;
using System.Collections;
using UnityEditor;

public class SerializedTest : MonoBehaviour {

//	[InitializeOnLoadMethod]
	private static void CheckProperties()
	{
		var so = new SerializedObject(Texture2D.whiteTexture);
		var iterator = so.GetIterator();
		while (iterator.NextVisible(true))
		{
			Debug.Log(iterator.name+" | "+iterator.propertyPath+" | "+iterator.propertyType+" | "+iterator.displayName);
		}
	}
}
