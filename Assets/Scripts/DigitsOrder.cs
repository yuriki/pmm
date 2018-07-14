using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitsOrder : MonoBehaviour
{

	public Sprite back;
	public Sprite forward;

	Toggle toggle;
	bool isBackwards = true;

	public void DigitsOrderToggle ()
	{
		toggle = GetComponent<Toggle>();
		if (isBackwards)
		{
			toggle.targetGraphic.GetComponentInChildren<Image>().sprite = forward;
			isBackwards = false;
		}
		else
		{
			toggle.targetGraphic.GetComponentInChildren<Image>().sprite = back;
			isBackwards = true;
		}
	}

}
