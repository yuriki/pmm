using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoURL : MonoBehaviour
{
	public string urlText;
	string url;

	public void GoUrlLocalized()
	{
		url = Lean.Localization.LeanLocalization.GetTranslationText(urlText);
		Application.OpenURL(url);
		Debug.Log("url: " + url);
	}
}
