using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoLevelsToggle : MonoBehaviour
{
	public GameObject SecondLevelTogglesHolder;
	public GameObject[] otherButtons;
	public GameObject[] otherButtonsPos;

	[Header("Settings for timing")]
	public float distanceMyltiplier;
	public float time;

	[Header("Type of example this button generates")]
	public ExampleTypeData exampleType; //corresponds ExampleSwitch StateData (read Developer Description)

	Text label;
	bool isSecondLevel;
	Vector3 scale;
	Vector3 direction;


	void Start()
	{
		label = this.transform.Find("Label").GetComponent<Text>();
		label.text = exampleType.buttonName;
	}


	public void GoToLevelTwo()
	{
		if(isSecondLevel)
		{
			SaveTogglesStates(); //TODO save toggle states
			this.GetComponent<LoadLevel>().LoadScene(2);
		}

		if (!isSecondLevel)
		{
			ChangeScale(0.7f);
			iTween.PunchScale(this.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time*0.7));
			ToggleSecondLevelBtns();
			label.text = exampleType.shortButtonName;

			//spread buttons around asunder
			for (int i = 0; i < otherButtons.Length; i++)
			{
				//making button NONinteractable (because someone can push two or more buttons together with two or more fingers)
				otherButtons[i].GetComponentInChildren<Button>().interactable = false;

				direction = Vector3.Normalize(otherButtons[i].transform.position - this.transform.position);

				iTween.MoveTo(otherButtons[i], iTween.Hash("x", distanceMyltiplier * direction.x, "y", distanceMyltiplier * direction.y, "time", time, "easetype", "easeInQuad"));
			}
			isSecondLevel = true;
		}
	}


	void SaveTogglesStates()
	{

	}


	public void BackToLevelOne()
	{
		ChangeScale(1f);
		iTween.PunchScale(this.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", time*0.7));

		label.text = exampleType.buttonName;
		ToggleSecondLevelBtns();

		//brings all buttons around back
		for (int i = 0; i < otherButtons.Length; i++)
		{
			//making button interactable back again
			otherButtons[i].GetComponentInChildren<Button>().interactable = true;

			iTween.MoveTo(otherButtons[i], iTween.Hash("x", otherButtonsPos[i].transform.position.x, "y", otherButtonsPos[i].transform.position.y, "time", time));
		}

		isSecondLevel = false;
	}


	void ChangeScale(float scaleValue)
	{
		scale = this.transform.localScale;
		scale.x = scale.y = scaleValue;
		this.transform.localScale = scale;
		label.transform.gameObject.transform.localScale = new Vector3(1/scaleValue, 1/scaleValue, 1/scaleValue);
	}


	void ToggleSecondLevelBtns()
	{
		iTween.PunchScale(SecondLevelTogglesHolder, iTween.Hash("x", 0.9f, "y", 0.9f, "time", time));
		SecondLevelTogglesHolder.SetActive(!SecondLevelTogglesHolder.activeInHierarchy);
	}

}
