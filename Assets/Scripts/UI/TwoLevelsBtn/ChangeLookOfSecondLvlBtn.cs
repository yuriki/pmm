using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLookOfSecondLvlBtn: MonoBehaviour
{
	public ButtonColorData baseColor;

	Image secondLevelImage;

	//I need Awake here (and not Start) because in ToggleOption I'm using Start to change toggle value
	private void Awake()
	{
		secondLevelImage = this.GetComponent<Image>();
	}


	public void ColorToggle(bool toggleOn)
	{
		if (toggleOn)
		{
			secondLevelImage.color = baseColor.color; //new Color(0.92f, 0.43f, 0.77f, 1f);
			this.transform.localScale = new Vector3(1f, 1f, 1f);
			iTween.PunchScale(this.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.2f));
		}
		else
		{
			secondLevelImage.color = new Color(0.6f, 0.6f, 0.6f, 1f);
			this.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			iTween.PunchScale(this.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", 0.2f));
		}
	}

}
