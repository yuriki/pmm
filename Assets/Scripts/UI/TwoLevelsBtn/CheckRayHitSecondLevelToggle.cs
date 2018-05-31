using UnityEngine.UI;
using UnityEngine;

public class CheckRayHitSecondLevelToggle : MonoBehaviour
{
	public Camera cam;

	GameObject prevObj;
	bool mouseDown;
	bool firstToggleChanged;
	bool currentStateForAllFurtherToggles;


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
		//TODO add second check - if SecondLevelButtons ACTIVE (because I don't want to calculate Raycasts needlessly)
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
						//one toggle must be always ON. I did this inside ExampleGenerator: "else" option always has one default option
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
