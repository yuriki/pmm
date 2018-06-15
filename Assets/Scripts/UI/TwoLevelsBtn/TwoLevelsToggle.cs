using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TwoLevelsToggle : MonoBehaviour
{
	public StateData exampleSwitch;
	public GameObject SecondLevelTogglesHolder;
	public GameObject[] otherButtons;
	public GameObject[] otherButtonsPos;
	public AudioSource dontDestroySound;

	[Header("Settings for timing")]
	float distanceMyltiplier = 8;
	float timeForButtonsAround = 0.2f;
	float timeMainButton = 0.5f;

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
			dontDestroySound.Play();
			//TODO save toggle states
			//SaveTogglesStates(); 
			exampleSwitch.Value = exampleType.exampleType;
			SceneManager.LoadScene(2);
		}

		if (!isSecondLevel)
		{
			this.GetComponent<AudioSource>().Play();

			iTween.Stop();
			ChangeScale(0.7f);
			iTween.PunchScale(this.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", timeMainButton));
			ToggleSecondLevelBtns(false);
			label.text = exampleType.shortButtonName;

			//spread buttons around asunder
			for (int i = 0; i < otherButtons.Length; i++)
			{
				//making button NONinteractable (because someone can push two or more buttons together with two or more fingers)
				otherButtons[i].GetComponentInChildren<Button>().interactable = false;

				direction = Vector3.Normalize(otherButtons[i].transform.position - this.transform.position);

				iTween.MoveTo(otherButtons[i], iTween.Hash("x", distanceMyltiplier * direction.x, "y", distanceMyltiplier * direction.y, "time", timeForButtonsAround, "easetype", "easeInQuad"));
			}
			isSecondLevel = true;
		}
	}


	void SaveTogglesStates()
	{

	}


	public void BackToLevelOne()
	{
		iTween.Stop(this.gameObject);
		ChangeScale(1f);
		iTween.PunchScale(this.gameObject, iTween.Hash("x", 0.5f, "y", 0.5f, "time", timeMainButton));

		label.text = exampleType.buttonName;
		ToggleSecondLevelBtns(true);

		//brings all buttons around back
		for (int i = 0; i < otherButtons.Length; i++)
		{
			//making button interactable back again
			otherButtons[i].GetComponentInChildren<Button>().interactable = true;

			iTween.MoveTo(otherButtons[i], iTween.Hash("x", otherButtonsPos[i].transform.position.x, "y", otherButtonsPos[i].transform.position.y, "time", timeForButtonsAround));
		}

		isSecondLevel = false;
	}


	void ChangeScale(float scaleValue)
	{
		scale = this.transform.localScale;
		scale.x = scale.y = scaleValue;
		this.transform.localScale = scale;
		//To remain LABEL size the same (after I scaled BUTTON DOWN) I need to scale LABEL UP
		label.transform.gameObject.transform.localScale = new Vector3(1/scaleValue, 1/scaleValue, 1/scaleValue);
	}


	void ToggleSecondLevelBtns(bool isBackToFirstLevel)
	{
		SecondLevelTogglesHolder.SetActive(!SecondLevelTogglesHolder.activeInHierarchy);
	}
}
