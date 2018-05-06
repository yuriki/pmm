using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class Skin : MonoBehaviour 
{
	public ButtonSkinData buttonSkin;
	public ButtonTypeData buttonType;

	protected virtual void OnSkinUI()
	{
		
	}

	public virtual void Awake ()
	{
		OnSkinUI ();
	}

	//TODO you have to write custom editor script to do this thing (to not do this check every Update)
	public virtual void Update()
	{
		if (Application.isEditor)
		{
			OnSkinUI();
		}
	}
}
