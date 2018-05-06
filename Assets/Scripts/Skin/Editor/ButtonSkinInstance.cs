using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ButtonSkinInstance : Editor 
{
	[MenuItem ("GameObject/SkinedButtons/Button", priority=0)]
	public static void AddButton ()
	{
		Create ("MainButton");
	}

	static GameObject clickedObject;

	private static GameObject Create (string objectName)
	{
		GameObject instance = Instantiate (Resources.Load<GameObject> (objectName));
		instance.name = objectName;
		clickedObject = UnityEditor.Selection.activeObject as GameObject;
		if (clickedObject != null)
		{
			instance.transform.SetParent (clickedObject.transform, false);
		}
		return instance;
	}
}
