using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSwitch : MonoBehaviour
{
	public StateData exampleSwitch;

	public void ChooseExampleType(int type)
	{
		exampleSwitch.Value = type;
	}
}
