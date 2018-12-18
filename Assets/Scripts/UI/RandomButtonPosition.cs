using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomButtonPosition : MonoBehaviour
{
	public Camera cam;
	public float multiplier = 0.045f;
	public HorizontalLayoutGroup grid;
	[Space]
	public GameObject[] buttons;

	int screenWidth;
	int randomButtonID;

	private void OnEnable()
	{
		SetSpacing();

		MoveRandomButtonOnTopHierarchy();
		//MoveRandomButtonOnTopHierarchy();
	}

	private void MoveRandomButtonOnTopHierarchy()
	{
		randomButtonID = Random.Range(0, 3);
		if (randomButtonID == 0)
		{
			if (Random.Range(0, 2) == 0)
			{
				buttons[randomButtonID].transform.SetSiblingIndex(randomButtonID);
			}
		}
		else
		{
			buttons[randomButtonID].transform.SetSiblingIndex(0);
		}
		
	}

	private void SetSpacing()
	{
		screenWidth = cam.pixelWidth;
		grid = this.GetComponent<HorizontalLayoutGroup>();
		grid.spacing = screenWidth * multiplier;
	}
}
