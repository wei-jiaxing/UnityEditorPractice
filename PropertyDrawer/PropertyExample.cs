using UnityEngine;

public class PropertyExample : MonoBehaviour
{
	[BigHeight]
	public string nickName;

	[BigHeight]
	public GameObject level;

	[BigHeight]
	public Texture icon;
}

[System.AttributeUsage (System.AttributeTargets.Field,
	Inherited = true, AllowMultiple = true)]
public class BigHeightAttribute : PropertyAttribute
{
	
}