using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MultTableToggle : MonoBehaviour
{
	public GameObject SmallFlowerTogglesHolder;
	public Text label;
	public GameObject generalObject;
	public Camera cam;
	public GameObject[] otherButtons;

	[Header("Settings for timing")]
	public float distanceMyltiplier;
	public float time;

	Vector3 scale;
	bool mouseDown;
	bool secondLevelToggle;
	bool firstToggleChanged;
	bool currentStateForAllFurtherToggles;
	GameObject prevObj;
	Vector3 direction;
	Vector3 tmp;


	public void GoToLevelTwo()
	{
		if(secondLevelToggle)
		{
			generalObject.GetComponent<LoadLevel>().LoadScene(2);
		}

		if (!secondLevelToggle)
		{
			ChangeScale(0.7f);
			ToggleSmallFlowerBtns();
			label.text = "2×..";

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
			secondLevelToggle = true;
		}
	}


	public void BackToLevelOne()
	{
		ChangeScale(1f);
		
		label.text = "2×3=?";
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

		secondLevelToggle = false;
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


	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			mouseDown = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			mouseDown = false;
			prevObj = null;
			firstToggleChanged = false;
		}
	}


	private void FixedUpdate()
	{
		if (mouseDown)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y), -Vector2.up, 0f);
			if (hit.collider != null)
			{
				//if objects are different
				if (!GameObject.ReferenceEquals(prevObj, hit.transform.gameObject))
				{
					prevObj = hit.transform.gameObject;

					if (!firstToggleChanged)
					{
						prevObj.GetComponent<Toggle>().isOn = !prevObj.GetComponent<Toggle>().isOn;
						currentStateForAllFurtherToggles = prevObj.GetComponent<Toggle>().isOn;
						firstToggleChanged = true;
					}

					if (firstToggleChanged)
					{
						prevObj.GetComponent<Toggle>().isOn = currentStateForAllFurtherToggles;
					}
				}
			} 
		}
	}


}
