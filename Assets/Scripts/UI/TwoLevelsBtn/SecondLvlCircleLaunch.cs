using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondLvlCircleLaunch : MonoBehaviour
{
	[Tooltip ("Array must be filled accordingly to toggles (ExampleToggleData) order inside Switches")]
	public GameObject[] secondLvlButtons;
	public ExampleTogglesData switches;

	float [] scale = new float[11];

	void OnEnable ()
	{
		for (int i = 0; i < secondLvlButtons.Length; i++)
		{
			//set scale of SecondLvlBtn according to current state (ON/OFF)
			if (i != (secondLvlButtons.Length - 1))		//for all SecondLvlBtns except last - BackToLevelOne_Btn
			{
				//if ON
				if (switches.toggles[i])
				{
					scale[i] = 1;
				}
				//if OFF
				else
				{
					scale[i] = 0.8f;
				} 
			}
			else										//for BackToLevelOne button
			{
				scale[i] = 1;
			}

			if (secondLvlButtons[i])
			{
				StartCoroutine(WaitSetActiveAndWobble(i));
			}
		}
	}


	IEnumerator WaitSetActiveAndWobble(int id)
	{
		yield return new WaitForSeconds(id * 0.025f);
		secondLvlButtons[id].SetActive(true);
		iTween.ScaleTo(secondLvlButtons[id], iTween.Hash("x", scale[id], "y", scale[id], "time", 0.5f/*, "delay", i * 0.03f*/, "easetype", "easeOutElastic"));
	}


	void OnDisable()
	{
		for (int i = 0; i < secondLvlButtons.Length; i++)
		{
			if (secondLvlButtons[i])
			{
				secondLvlButtons[i].SetActive(false);
				secondLvlButtons[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f); 
			}
		}
	}
}
