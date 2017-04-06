using UnityEngine;
using System.Collections.Generic;

public class PropertyDrawerSample : MonoBehaviour
{
	public Sample sample;
	public List<Sample> samples;
}

[System.Serializable]
public class Sample
{
	public string name;
	public int level;
	public Job job;

	public enum Job
	{
		Soldier,
		Wizard,
		Thief,
	}

	public Sample()
	{
		name = "hoge";
		level = 1;
		job = Job.Wizard;
	}
}
