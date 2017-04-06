using UnityEngine;
using UnityEditor;

public class WizardWindow : ScriptableWizard {
	public GameObject gameObj;
	public UIAtlas atlas;
	private GameObject selection;

	static WizardWindow window;
	[MenuItem("Editor Example/WizardWindow")]
	public static void Start () {
		window = DisplayWizard<WizardWindow>("Atlas Switch", "Switch", "Reset");
		if (Selection.activeGameObject != null)
			window.gameObj = Selection.activeGameObject;
	}

	[MenuItem("Editor Example/r_WizardWindow", true)]
	static bool ValidateStart()
	{
		return Selection.activeGameObject != null;
	}

	void OnWizardCreate()
	{
		foreach (var sprite in gameObj.GetComponentsInChildren<UISprite>(true))
		{
			sprite.atlas = atlas;
			sprite.MakePixelPerfect();
		}
		Debug.Log("Switch Atlas Complete");
		Close();
	}

	void OnWizardOtherButton()
	{
		gameObj = null;
		atlas = null;
		//しないと、OnWizardUpdateしない
		OnWizardUpdate();
	}

	void OnWizardUpdate()
	{
		Debug.Log("update");
		if (gameObj != null && atlas != null)
		{
			Debug.Log("true");
			isValid = true;
			helpString = "";
		}
		else
		{
			helpString = "Input the Go and Atlas";
			isValid = false;
		}
	}

	//普通のEditorGUIを使うと、プロパティーがなくなる
//	int a;
//	protected override bool DrawWizardGUI ()
//	{
//		a = EditorGUILayout.IntField("Label",a);
//		if (a != 3)
//		{
//			return false;
//		}
//		else
//		//false を返すことで OnWizardUpdate が呼び出されなくなる
//			return true;
//	}
}
