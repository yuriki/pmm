using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoLevelsToggle : MonoBehaviour
{
	public GameObject SmallFlowerTogglesHolder;
	public GameObject[] otherButtons;

	[Header("Settings for timing")]
	public float distanceMyltiplier;
	public float time;

	[Header("Type of example this button generates")]
	public ExampleTypeData exampleType; //corresponds ExampleSwitch StateData (read Developer Description)

	Text label;
	bool isSecondLevel;
	Vector3 scale;
	Vector3 direction;
	Vector3 tmp;


	void Start()
	{
		label = this.transform.Find("Label").GetComponent<Text>();
		label.text = exampleType.buttonName;
	}


	public void GoToLevelTwo()
	{
		if(isSecondLevel)
		{
			SaveTogglesStates(); //TODO
			this.GetComponent<LoadLevel>().LoadScene(2);
		}

		if (!isSecondLevel)
		{
			ChangeScale(0.7f);
			ToggleSmallFlowerBtns();
			label.text = exampleType.shortButtonName;

			//spread buttons around asunder
			foreach (var button in otherButtons)
			{
				direction = Vector3.Normalize(button.transform.position - this.transform.position);

				//save x position as PosZ
				tmp = button.transform.position;
				tmp.z = button.transform.position.x;
				button.transform.position = tmp;

				//save y position as ScaleZ
				tmp.x = 1;
				tmp.y = 1;
				tmp.z = button.transform.position.y;
				button.transform.localScale = tmp;

				iTween.MoveTo(button, iTween.Hash("x", distanceMyltiplier*direction.x, "y", distanceMyltiplier*direction.y, "time", time, "easetype", "easeInQuad"));
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

		label.text = exampleType.buttonName;
		ToggleSmallFlowerBtns();

		//brings all buttons aroud back
		foreach (var button in otherButtons)
		{
			direction.x = button.transform.position.z;
			direction.y = button.transform.localScale.z;
			direction.z = 0;
			iTween.MoveTo(button, iTween.Hash("x", direction.x, "y", direction.y, "z", direction.z, "time", time));

			button.transform.localScale = new Vector3 (1f, 1f, 1f);
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


	void ToggleSmallFlowerBtns()
	{
		SmallFlowerTogglesHolder.SetActive(!SmallFlowerTogglesHolder.activeInHierarchy);
	}

}
