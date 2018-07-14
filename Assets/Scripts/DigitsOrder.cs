using System;
using UnityEngine;
using UnityEngine.UI;

public class DigitsOrder : MonoBehaviour
{
	public Sprite back;
	public Sprite forward;
	public Text usersInputText;

	Toggle flipDigitsToggle;

	public void FlipDigitsOrderToggle ()
	{
		usersInputText.text = Reverse(usersInputText.text);

		flipDigitsToggle = GetComponent<Toggle>();
		if (flipDigitsToggle.isOn)
		{
			flipDigitsToggle.targetGraphic.GetComponentInChildren<Image>().sprite = forward;
		}
		else
		{
			flipDigitsToggle.targetGraphic.GetComponentInChildren<Image>().sprite = back;
		}
	}


	string Reverse(string s)
	{
		char[] charArray = s.ToCharArray();
		Array.Reverse(charArray);
		return new string(charArray);
	}
}
