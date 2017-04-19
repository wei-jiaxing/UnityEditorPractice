using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// Another way to preview UISprite's animation, and multiple objects can be used this time.
/// 
/// For using multiple, "EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);" in NGUIEditorTools's
/// DrawSprite() function need to be deleted or changed to proper code, because there cannnot use a fixed Rect for multiple objects.
/// 
/// [!Pay Attention!] UISprite's spriteName will be changed when previewing.
/// </summary>
[CustomEditor(typeof(UISprite))]
[CanEditMultipleObjects]
public class UISpriteAnimator : UISpriteInspector
{
	Dictionary<UISprite, AnimationSetting> _spriteAnims = new Dictionary<UISprite, AnimationSetting>();

	bool _isPlaying = true;
	bool _hasAnimation = false;

	protected override void OnEnable()
	{
		base.OnEnable();
		_hasAnimation = false;
		if (targets != null)
		{
			foreach (var target in targets)
			{
				var targetSprite = target as UISprite;
				var spriteAnim = targetSprite.GetComponent<UISpriteAnimation>();
				if (spriteAnim != null)
				{
					_spriteAnims[targetSprite] = new AnimationSetting(spriteAnim);
					_hasAnimation = true;
				}
			}
		}
	}

	public override void OnPreviewGUI(Rect rect, GUIStyle background)
	{
		var t = target as UISprite;
		AnimationSetting setting;

		if (!Application.isPlaying && _spriteAnims.TryGetValue(t, out setting))
		{
			setting.delta += ((float)EditorApplication.timeSinceStartup - setting.lastTime) * _speedScale;
			setting.lastTime = (float)EditorApplication.timeSinceStartup;

			var spriteAnim = setting.anim;

			BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
			var field = typeof(UISpriteAnimation).GetField("mSpriteNames",flags);
			var spriteNames = field.GetValue(spriteAnim) as List<string>;

			if (spriteNames.Count > 0)
			{
				if (_isPlaying)
				{
					float rate = 1f / spriteAnim.framesPerSecond;
					if (rate < setting.delta)
					{
						setting.delta = Mathf.Repeat(setting.delta, rate);
						setting.index++;
					}
					setting.index %= spriteNames.Count;
					t.spriteName = spriteNames[setting.index];
				}

				EditorGUI.DropShadowLabel(rect, spriteNames[setting.index]);
				rect.height -= 15;
			}
			else
			{
				return;
			}
		}

		base.OnPreviewGUI(rect, background);
	}

	public override void OnPreviewSettings()
	{
		base.OnPreviewSettings();
		if (!_hasAnimation)
		{
			return;
		}

		var playButton = EditorGUIUtility.IconContent("preAudioPlayOn");
		var pauseButton = EditorGUIUtility.IconContent("preAudioPlayOff");

		EditorGUI.BeginChangeCheck();
		_isPlaying = GUILayout.Toggle(_isPlaying, _isPlaying ? playButton : pauseButton, (GUIStyle)"preButton");
		if (EditorGUI.EndChangeCheck())
		{
			foreach (var setting in _spriteAnims.Values)
			{
				setting.lastTime = (float)EditorApplication.timeSinceStartup;
			}
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
		var speedScale = EditorGUIUtility.IconContent("SpeedScale", "Speed Scale");
		if (GUILayout.Button(speedScale, (GUIStyle)"preButton"))
		{
			_speedScale = 1;
		}
		_speedScale = GUILayout.HorizontalSlider(_speedScale, 0f, 5f, (GUIStyle)"preSlider", (GUIStyle)"preSliderThumb");

		GUILayout.Box(_speedScale.ToString("0.000"), new GUIStyle("preLabel"));
	}
	float _speedScale = 1f;

	public override bool HasPreviewGUI()
	{
		return true;
	}

	public override bool RequiresConstantRepaint()
	{
		return _isPlaying;
	}

	public class AnimationSetting
	{
		public int index;
		public float delta;
		public float lastTime;
		public UISpriteAnimation anim;

		public AnimationSetting(UISpriteAnimation anim)
		{
			index = 0;
			delta = 0;
			lastTime = (float)EditorApplication.timeSinceStartup;
			this.anim = anim;
		}
	}
}

/// <summary>
/// List the all UISprites used in UISprieAnimation
/// 
/// For using this, "EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);" in NGUIEditorTools's 
/// DrawSprite() function need to be deleted or changed to proper code, because there cannnot use a fixed Rect for multiple objects.
/// </summary>
[CustomPreview(typeof(UISprite))]
public class UISpriteAnimationList : ObjectPreview
{
	public override bool HasPreviewGUI()
	{
		return true;
	}

	public override void Initialize(Object[] targets)
	{
		base.Initialize(targets);
		List<Object> sprites = new List<Object>();
		foreach (UISprite target in targets)
		{
			var anim = target.GetComponent<UISpriteAnimation>();
			if (anim != null)
			{
				BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
				var field = typeof(UISpriteAnimation).GetField("mSpriteNames", flags);
				var spriteNames = field.GetValue(anim) as List<string>;

				foreach (string name in spriteNames)
				{
					UISprite newSprite = new GameObject(name).AddComponent<UISprite>();
					newSprite.gameObject.hideFlags = HideFlags.HideAndDontSave;
					newSprite.atlas = target.atlas;
					newSprite.spriteName = name;
					newSprite.enabled = false;
					sprites.Add(newSprite);
				}
			}
		}
		m_Targets = sprites.ToArray();
	}

	public override GUIContent GetPreviewTitle()
	{
		return new GUIContent("Sprite List");
	}

	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		UISprite sprite = target as UISprite;
		if (sprite == null || !sprite.isValid) return;

		Texture2D tex = sprite.mainTexture as Texture2D;
		if (tex == null) return;

		UISpriteData sd = sprite.atlas.GetSprite(sprite.spriteName);
		NGUIEditorTools.DrawSprite(tex, rect, sd, sprite.color);
	}
