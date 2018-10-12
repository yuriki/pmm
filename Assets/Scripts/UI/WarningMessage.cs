using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour
{
	public GameObject warningWindow;
	public GameObject exitButton;
	public Dropdown currenciesDropdown;
	public GameObject paramsPanel;
	
	public void ShowWarningMessage(int optionID)
	{
		if (optionID != 6)
		{
			warningWindow.SetActive(true);
			exitButton.SetActive(false);

			DeleteDropdownList();
			paramsPanel.SetActive(false);
		}
	}

	void DeleteDropdownList()
	{
		Transform dropdownList;
		dropdownList = paramsPanel.transform.Find("UI_AdminDropdownItem/Dropdown/Dropdown List");
		if (dropdownList != null)
		{
			Destroy(dropdownList.gameObject);
		}
	}

	public void HideWarningWindow(bool doRevert)
	{
		warningWindow.SetActive(false);
		paramsPanel.SetActive(true);
		exitButton.SetActive(true);

		if (doRevert)
		{
			RevertDropdownMenu();
		}
	}

	void RevertDropdownMenu()
	{
		currenciesDropdown.value = 6;
	}
}
