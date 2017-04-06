using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class EditorGUILayoutWindow : EditorWindow , IHasCustomMenu
{
	bool flag = true;
	public void AddItemsToMenu (GenericMenu menu)
	{
		menu.AddItem (new GUIContent ("example"), flag, () => {
			window.titleContent = new GUIContent("Exapmle");
			flag = true;
		});

		menu.AddItem (new GUIContent ("example2"), !flag, () => {
			window.titleContent = new GUIContent("Window");
			flag = false;
		});
		menu.AddSeparator("");
		menu.AddDisabledItem(new GUIContent("asfd"));
		menu.ShowAsContext();
		menu.AddItem(new GUIContent("reset"),false,()=>{window.position = new Rect(Screen.width / 2, 0, 750, 550);});
	}

	AnimBool bValue = new AnimBool(true);
	AnimFloat fAnim = new AnimFloat(1);
	static EditorGUILayoutWindow window;
	[MenuItem("Editor Example/EditorGUILayout")]
	public static void Start () {
		if (window == null)
			window = CreateInstance<EditorGUILayoutWindow>();
//		window.ShowUtility();
//		window.ShowAuxWindow();
//		window.ShowAsDropDown(new Rect(1,1,1,1),new Vector2(1100,800));
		// CustomMenuは普通のメニューだけ使えます
		window.Show();
	}

	void OnEnable()
	{
		bValue.valueChanged.AddListener(Repaint);
		bValue.speed = 5f;
		fAnim.valueChanged.AddListener(Repaint);
	}

	Texture2D obj;
	bool one, two, three;
	int id,id2;
	public float f;
	Rect buttonRect;
	void OnGUI()
	{
		var rect = EditorGUILayout.GetControlRect (false);
		rect = EditorGUI.PrefixLabel (rect, new GUIContent ("Select a mesh"));
		obj = EditorGUI.ObjectField (rect,
			"Calculate:",
			obj,
			typeof(Texture2D)) as Texture2D;
		GUILayout.Box("asdf");
		bValue.target = GUILayout.Toggle(bValue.target,"fade");

		if (EditorGUILayout.BeginFadeGroup(bValue.faded))
		{
			GUILayout.Button("asdfa");
			id = GUILayout.SelectionGrid(id,new string[]{"1","222","3"},5);
			id2 = GUILayout.Toolbar(id2,new string[]{"111","222","333"},"PreferencesKeysElement");
			EditorGUI.BeginDisabledGroup (!obj);
			GUILayout.Box("",GUILayout.Height(1),GUILayout.ExpandWidth(true));
			if(GUI.Button (EditorGUILayout.GetControlRect (), "Calculate!"))
				//			Calculate ();
				Debug.Log("Calculate");
			EditorGUI.EndDisabledGroup ();
		}
		EditorGUILayout.EndFadeGroup();

		fAnim.target = EditorGUILayout.Slider("Fade",fAnim.target,0.0f,1f);
		if (EditorGUILayout.BeginFadeGroup(fAnim.value))
		{
			EditorGUILayout.Knob(new Vector2(30,30), 50f,0,720,"aaaa",Color.black,Color.blue,false);
			EditorGUILayout.Toggle(true,EditorStyles.miniButtonRight);
			EditorGUILayout.Toggle(true,EditorStyles.toolbarButton);
			Color color = GUI.backgroundColor;
			if(!two) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
			GUILayout.Toggle(true,"button","dragtab");
			GUI.backgroundColor = color;
		}
		EditorGUILayout.EndFadeGroup();
		if (Event.current.type == EventType.Repaint)
		{
			GUI.skin.button.Draw(new Rect(100,400,100,100),new GUIContent("asdf"),0);
			GUI.skin.box.Draw(new Rect(1,400,10000,1),new GUIContent(""),0);
		}
		using (new EditorGUILayout.HorizontalScope ()) {
			EditorGUI.BeginChangeCheck();
			one = GUILayout.Toggle (one, "1", "ButtonLeft");
			if (EditorGUI.EndChangeCheck())
			{
				if(one)
					PopupWindow.Show(buttonRect, new PopupContentExample());
			}

			two = GUILayout.Toggle (two, "2", EditorStyles.miniButtonMid);
			two = GUILayout.Toggle (two, "2", EditorStyles.miniButtonMid);
			two = GUILayout.Toggle (two, "2", EditorStyles.miniButtonMid);
			two = GUILayout.Toggle (two, "2", EditorStyles.miniButtonMid);

			EditorGUI.BeginChangeCheck();
			three = GUILayout.Toggle (three, "3", EditorStyles.miniButtonRight);

			if (EditorGUI.EndChangeCheck())
			{
				if(three)
				{
					ShowPopupExample.Init();
				}
				else{
					ShowPopupExample.OnClose();
				}

			}
		}
		f = EditorGUILayout.Slider(f,0.0001f,1);
		EditorGUILayout.FadeGroupScope scope = new EditorGUILayout.FadeGroupScope(f);
		{
			GUILayout.Label("asfdfadsfdas");
			GUILayout.Label("asfdfadsfdas");
			GUILayout.Label("asfdfadsfdas");
			GUILayout.Label("asfdfadsfdas");
			GUILayout.Label("asfdfadsfdas");
			GUILayout.Label("asfdfadsfdas");
			GUILayout.Label("asfdfadsfdas");
		}
		scope.Dispose();
		GUILayout.Label("test");
		GUILayout.Label("test","AS TextArea");
		GUILayout.Label("test","AS TextArea");
		GUILayout.Label("test","AS TextArea");
		BeginWindows();
		GUILayout.Window(1,buttonRect,(unusedId)=>{GUILayout.Label("rockwjx");},"window");
		EndWindows();
		if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
	}
}
