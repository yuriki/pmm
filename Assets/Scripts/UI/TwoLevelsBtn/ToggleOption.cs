using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOption : MonoBehaviour
{
	public ExampleTogglesData options;
	public int id;


	//I need Start here (and not Awake) because in ChangeLookOfSecondLvlBtn I'm using Awake to reference component
	void Start()
	{
		this.GetComponent<Toggle>().isOn = options.toggles[id];
	}

	public void CheckOption(bool toggleOn)
	{
		if (toggleOn)
		{
			options.toggles[id] = true;
		}
		else
		{
			options.toggles[id] = false;
		}
	}

}
