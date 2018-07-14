using UnityEngine;
using UnityEngine.UI;

public class PressButton : MonoBehaviour
{
	
	public GameObject userInputTextObj;
	public Text questionMarkGreen;
	public GameObject questionMarkRed;
	public Text invisible;
	public AudioSource sound;

	[Header("States")]
	public NonRangedStateData correctAnswer;
	public StateData digitsNumber;

	Text userInputText;

	Vector3 scale = new Vector3 (0.6f,0.6f,0f);
	float time = 0.2f;

	string tmpInputString;
	int correctAnswerLength;

	private void Start()
	{
		userInputText = userInputTextObj.GetComponent<Text>();
	}


	public void PressDigit(int userInputDigit)
	{
		HideRedCross();
		correctAnswerLength = correctAnswer.Value.ToString().Length;

		if (userInputText.text == "")
		{
			userInputText.text = userInputDigit.ToString();

			if (correctAnswerLength > 1 && IsThisColumnExample())
				invisible.text = "<color=#DCDF71FF>?</color>" + "<color=#FFFFFF00>" + userInputDigit.ToString() + "</color>";
			else if(correctAnswerLength > 1)
				invisible.text = "<color=#FFFFFF00>" + userInputDigit.ToString() + "</color>" + "<color=#DCDF71FF>?</color>";

			WobbleUsersInputDigit();

			questionMarkGreen.text = "";
		}
		else if (userInputText.text.Length < digitsNumber.Value)
		{
			if (IsThisColumnExample())
			{
				tmpInputString = userInputText.text;
				userInputText.text = userInputDigit.ToString() + tmpInputString; //changing order of inputed digits

				if (correctAnswerLength == 3 && userInputText.text.Length < 3)
					invisible.text = "<color=#DCDF71FF>?</color>" + "<color=#FFFFFF00>" + userInputText.text + "</color>";
				else
					invisible.text = "";

				WobbleUsersInputDigit();
			}
			else
			{
				tmpInputString = userInputText.text;
				userInputText.text = tmpInputString + userInputDigit.ToString();

				if (correctAnswerLength == 3 && userInputText.text.Length < 3)
					invisible.text = "<color=#FFFFFF00>" + userInputText.text + "</color>" + "<color=#DCDF71FF>?</color>";
				else
					invisible.text = "";

				WobbleUsersInputDigit();
			}
		}
		else if (userInputText.text.Length >= digitsNumber.Value)
		{
			WobbleUsersInputDigit();
		}
	}


	void HideRedCross()
	{
		if (this.GetComponent<OnWrongAnswer>().crossHolder.activeInHierarchy)
		{
			this.GetComponent<OnWrongAnswer>().crossHolder.SetActive(false);
			userInputText.text = "";
		}
	}


	public void PressDelete()
	{
		tmpInputString = userInputText.text;
		int digitsNum = tmpInputString.Length;

		if (IsRedCrossHidden())
		{
			if (digitsNum == 1)
			{
				userInputText.text = tmpInputString.Remove(digitsNum - 1);
				questionMarkGreen.text = "?";
				invisible.text = "";
			}
			else if (digitsNum != 0)
			{
				//if this is Column example I changing order of removing digits
				if (IsThisColumnExample())
				{
					userInputText.text = tmpInputString.Remove(0, 1);

					//show faint question mark if length of correct answer longer than user input but not less than 1 
					//(because I show green question mark when user input is empty)
					if (correctAnswerLength > 1 && correctAnswerLength > userInputText.text.Length)
						invisible.text = "<color=#DCDF71FF>?</color>" + "<color=#FFFFFF00>" + userInputText.text + "</color>";
				}
				else
				{
					userInputText.text = tmpInputString.Remove(digitsNum - 1);

					//show faint question mark if length of correct answer longer than user input but not less than 1 
					//(because I show green question mark when user input is empty)
					if (correctAnswerLength > 1 && correctAnswerLength > userInputText.text.Length)
						invisible.text = "<color=#FFFFFF00>" + userInputText.text + "</color>" + "<color=#DCDF71FF>?</color>";
				}

				WobbleUsersInputDigit();
			} 
		}
		else
		{
			this.GetComponent<OnWrongAnswer>().crossHolder.SetActive(false);
			userInputText.text = "";
			questionMarkGreen.text = "?";
			invisible.text = "";
		}
	}


	bool IsRedCrossHidden()
	{
		return !this.GetComponent<OnWrongAnswer>().crossHolder.activeInHierarchy;
	}


	public void PressCheck()
	{
		if (HasUserInputSomethingAppropriate())
		{
			this.GetComponent<Check>().CheckResult();
			invisible.text = "";
		}
		else
		{
			sound.Play();
			iTween.PunchScale(questionMarkRed, iTween.Hash(
				"amount", new Vector3(0.5f, 0.5f, 0f),
				"time", 0.5f));
		}
	}


	bool HasUserInputSomethingAppropriate()
	{
		return userInputText.text != "" && !this.GetComponent<OnWrongAnswer>().crossHolder.activeInHierarchy;
	}


	void WobbleUsersInputDigit ()
	{
		iTween.PunchScale(userInputTextObj, scale, time);	
	}


	bool IsThisColumnExample()
	{
		return (this.GetComponent<ExampleGenerator>().exampleSwitch.Value == 2);
	}
}
