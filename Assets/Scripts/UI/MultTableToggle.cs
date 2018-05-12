using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultTableToggle : MonoBehaviour
{
	public GameObject SmallFlowerTogglesHolder;
	public Camera cam;

	Vector3 scale;
	bool mouseDown = false;
	GameObject prevObj;


	public void OnPressed(bool toggle)
	{

		if (toggle)
		{
			ChangeScale(0.5f);
			ToggleSmallFlowerBtns();
		}
		else
		{
			ChangeScale(1f);
			ToggleSmallFlowerBtns();
		}
	}


	void ChangeScale(float scaleValue)
	{
		scale = this.transform.localScale;
		scale.x = scale.y = scaleValue;
		this.transform.localScale = scale;
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
		}
	}

	private void FixedUpdate()
	{
		if (mouseDown)
		{
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y), -Vector2.up, 0f);
			if (hit/*.collider != null*/)
			{

				if (!GameObject.ReferenceEquals(prevObj, hit.transform.gameObject))
				{

					Debug.Log("Hit object name: " + hit.transform.name);
					prevObj = hit.transform.gameObject;
				}

				
			} 
		}
	}


}
