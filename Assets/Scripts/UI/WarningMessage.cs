using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour
{
	public GameObject warningWindow;
	public GameObject exitButton;
	public Dropdown currenciesDropdown;

	public void ShowWarningMessage(int optionID)
	{
		if (optionID != 6)
		{
			warningWindow.SetActive(true);
			exitButton.SetActive(false);
			Debug.Log(" Parents are responsible for paying out money"); 
		}
	}

	public void HideWarningWindow(bool doRevert)
	{
		
		warningWindow.SetActive(false);
		exitButton.SetActive(true);

		if (doRevert)
		{
			RevertDropdownMenu();
		}
	}

	public void RevertDropdownMenu()
	{
		currenciesDropdown.value = 6;
	}
}
