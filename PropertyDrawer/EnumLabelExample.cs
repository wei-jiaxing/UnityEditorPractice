using UnityEngine;
using System.Collections.Generic;

public class EnumLabelExample : MonoBehaviour
{
	public enum Job
	{
		[EnumLabel("戦士")]
		Warrior,

		[EnumLabel("魔法使い")]
		Wizard,

		[EnumLabel("盗賊")]
		Thief,
	}

	[EnumLabel("職業：")]
	public Job job;
}

[System.AttributeUsageAttribute(System.AttributeTargets.Field
	,Inherited = true, AllowMultiple = true)]
public class EnumLabelAttribute : PropertyAttribute
{
	public string displayName;

	public EnumLabelAttribute(string displayName)
	{
		this.displayName = displayName;
	}
}

