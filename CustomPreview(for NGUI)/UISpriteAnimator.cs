using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Another way to preview UISprite's animation, and multiple objects can be used this time.
/// 
/// For using multiple, "EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);" in NGUIEditorTools
/// should be deleted because there cannnot use a label for multiple objects.
/// 
/// [!Pay Attention!] UISprite's spriteName was changed when previewing.
/// </summary>
[CustomEditor(typeof(UISprite))]
[CanEditMultipleObjects]
public class UISpriteAnimator : UISpriteInspector
{
	Dictionary<UISprite,UISpriteAnimation> _spriteAnims = new Dictionary<UISprite, UISpriteAnimation>();

	float _delta;
	int _index;
	float _lastTime;
	bool _isPlaying = true;
	bool _isMultiply;

	protected override void OnEnable()
	{
		base.OnEnable();
		if (targets != null)
		{
			foreach (var target in targets)
			{
				var targetSprite = target as UISprite;
				var spriteAnim = targetSprite.GetComponent<UISpriteAnimation>();
				if (spriteAnim != null)
				{
					_spriteAnims[targetSprite] = spriteAnim;
					_lastTime = (float)EditorApplication.timeSinceStartup;
				}
			}
			_isMultiply = targets.Length > 1;
		}
	}

	public override void OnPreviewGUI(Rect rect, GUIStyle background)
	{
		_delta += ((float)EditorApplication.timeSinceStartup - _lastTime) * _speedScale;
		_lastTime = (float)EditorApplication.timeSinceStartup;

		var t = target as UISprite;
		UISpriteAnimation spriteAnim;

		if (_spriteAnims.TryGetValue(t, out spriteAnim))
		{
			float rate = 1f / spriteAnim.framesPerSecond;
			if (rate < _delta)
			{
				_delta = Mathf.Repeat(_delta, rate);
				_index++;
			}

			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
			var field = typeof(UISpriteAnimation).GetField("mSpriteNames",flags);
			var spriteNames = field.GetValue(spriteAnim) as List<string>;

			int idx = _index % spriteNames.Count;
			if (idx < spriteNames.Count && _isPlaying)
				t.spriteName = spriteNames[idx];
			
			if (!_isMultiply)
			{
				EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(1f, 18f), spriteNames[idx]);
			}
		}

		base.OnPreviewGUI(rect, background);
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
		if (_isPlaying)
		{
			// Same as RequiresConstantRepaint(){return true;}
//			foreach (var activeEditor in ActiveEditorTracker.sharedTracker.activeEditors)
//			{
//				activeEditor.Repaint();
//			}
		}

		// Speed Scale
		//		var speedScale = EditorGUIUtility.IconContent("SpeedScale", "Speed Scale");
		//		GUILayout.Box(speedScale, (GUIStyle)"preButton");
		//		_speedScale = GUILayout.HorizontalSlider(_speedScale, 0f, 5f, (GUIStyle)"preSlider", (GUIStyle)"preSliderThumb");
		//
		//		GUILayout.Box(_speedScale.ToString("0.000"), new GUIStyle("preLabel"));
	}
	float _speedScale = 1f;

	public override bool HasPreviewGUI()
	{
		return true;
	}

	public override bool RequiresConstantRepaint()
	{
		return true;
	}
}
