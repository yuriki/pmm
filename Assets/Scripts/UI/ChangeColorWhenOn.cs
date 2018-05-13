using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorWhenOn : MonoBehaviour
{
	public Image image;
	public void ColorToggle(bool toggleOn)
	{
		if (toggleOn)
		{
			image.color = new Color(0.92f, 0.43f, 0.77f, 1f); 
		}
		else
		{
			image.color = new Color(0.64f, 0.64f, 0.64f, 1f);
		}
	}

}
