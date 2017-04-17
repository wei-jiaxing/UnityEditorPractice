using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// UISpriteAnimation component's animation previewer
/// Usage: Select the GameObject that has UISpriteAnimation component,
///        and change the previewer's title below to "Sprite Animation" in the Inspector window
/// 
/// Because using NGUIEditorTools.DrawSprite(), this cannot support multiple GameObjects
/// </summary>
[CustomEditor(typeof(UISpriteAnimation))]
public class SpriteAnimationShower : UISpriteAnimationInspector
{
	UISpriteAnimation t;
	UISprite _sprite;

	float _delta;
	int _index;
	float _lastTime;
	bool _isPlaying = true;

	private void OnEnable()
	{
		if (target != null)
		{
			t = target as UISpriteAnimation;
			_sprite = t.GetComponent<UISprite>();
			_lastTime = (float)EditorApplication.timeSinceStartup;
			EditorApplication.update -= Update;
			EditorApplication.update += Update;
		}
	}

	private void OnDisable()
	{
		EditorApplication.update -= Update;
	}

	private void Update()
	{
		if (!_isPlaying)
			return;
		
		_delta += ((float)EditorApplication.timeSinceStartup - _lastTime) * _speedScale;
		_lastTime = (float)EditorApplication.timeSinceStartup;
		float rate = 1f / t.framesPerSecond;
		if (rate < _delta)
		{
			_delta = Mathf.Repeat(_delta, rate);
			if (++_index >= t.frames)
			{
				_index = 0;
			}
		}
	}

	public override void OnPreviewGUI(Rect r, GUIStyle background)
	{
		if (_sprite == null || !_sprite.isValid)
			return;

		Texture2D tex = _sprite.mainTexture as Texture2D;
		if (tex == null)
			return;

		BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
		var field = typeof(UISpriteAnimation).GetField("mSpriteNames",flags);
		var spriteNames = field.GetValue(t) as List<string>;

		if (_index >= spriteNames.Count)
			return;

		UISpriteData sd = _sprite.atlas.GetSprite(spriteNames[_index]);
		EditorGUILayout.LabelField(spriteNames[_index]);
		NGUIEditorTools.DrawSprite(tex, r, sd, _sprite.color);
	}

	public override GUIContent GetPreviewTitle()
	{
		return new GUIContent("Sprite Animation");
	}

	public override bool HasPreviewGUI()
	{
		return true;
	}

	public override bool RequiresConstantRepaint()
	{
		return true;
	}

	public override void OnPreviewSettings()
	{
		base.OnPreviewSettings();
		var playButton = EditorGUIUtility.IconContent("preAudioPlayOn");
		var pauseButton = EditorGUIUtility.IconContent("preAudioPlayOff");

		EditorGUI.BeginChangeCheck();
		_isPlaying = GUILayout.Toggle(_isPlaying, _isPlaying ? playButton : pauseButton, (GUIStyle)"preButton");
		if (EditorGUI.EndChangeCheck())
		{
			_lastTime = (float)EditorApplication.timeSinceStartup;
		}

		// Speed Scale
//		var speedScale = EditorGUIUtility.IconContent("SpeedScale", "Speed Scale");
//		GUILayout.Box(speedScale, (GUIStyle)"preButton");
//		_speedScale = GUILayout.HorizontalSlider(_speedScale, 0f, 5f, (GUIStyle)"preSlider", (GUIStyle)"preSliderThumb");
//
//		GUILayout.Box(_speedScale.ToString("0.000"), new GUIStyle("preLabel"));
	}

	float _speedScale = 1f;
}

/// <summary>
/// If this Object has the UISpriteAnimation component, hide the default UISprite preview
/// </summary>
[CustomEditor(typeof(UISprite))]
public class UISpriteAnimator : UISpriteInspector
{
	public override bool HasPreviewGUI()
	{
		var t = target as UISprite;
		var anim = t.GetComponent<UISpriteAnimation>();
		if (anim != null)
			return false;
		else
			return base.HasPreviewGUI();
	}
}