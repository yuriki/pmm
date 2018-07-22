using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableSimpleExamples : MonoBehaviour
{
	public BoolData isHardExamplesOnly;
	public ExampleTogglesData [] secondLevelToggesArray;
	public InputField rewawrdCountTo10ExampleInput;
	public GameObject generalObj;

	private void Awake()
	{
		gameObject.GetComponent<Toggle>().isOn = isHardExamplesOnly.toggle;
	}

	public void HardExamplesOnlyToggle(bool toggle)
	{
		isHardExamplesOnly.toggle = toggle;

		if (toggle)
		{
			rewawrdCountTo10ExampleInput.text = "0";
			generalObj.GetComponent<FillAdminData>().SaveAllRewardsToFile();

			foreach (var secondLevelTogges in secondLevelToggesArray)
			{
				for (int i = 0; i < secondLevelTogges.toggles.Length; i++)
				{
					secondLevelTogges.toggles[i] = true;
				} 
			}
			//TODO when I go to 2_Example_template scene isHardExamplesOnly changed to OFF. WHY????
		}

		generalObj.GetComponent<LoadCreateSaveLocally>().SaveJSONLocally(generalObj.GetComponent<LoadCreateSaveLocally>().disableSimpleExamples_FileName, isHardExamplesOnly);
	}
	
}
