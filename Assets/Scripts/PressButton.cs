using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(StretchRect))]
public class PressButton : MonoBehaviour
{
	public AudioSource sound;
	public Toggle flipDigitsToggle;

	[Header("Objects to wobble")]
	public GameObject userInputTextObj;
	public GameObject questionMarkRed;

	[Header("Text")]
	public Text questionMarkGreen;
	public Text invisible;

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

			//formating invisible string to match visible user input cause I need precisely indicate next position for user input digit
			if (correctAnswerLength > 1 && IsThisColumnExample())
				invisible.text = "<color=#DCDF71FF>?</color>" + "<color=#FFFFFF00>" + userInputDigit.ToString() + "</color>";
			else if(correctAnswerLength > 1)
				invisible.text = "<color=#FFFFFF00>" + userInputDigit.ToString() + "</color>" + "<color=#DCDF71FF>?</color>";

			questionMarkGreen.text = "";
		}
		else if (userInputText.text.Length < digitsNumber.Value)
		{
			tmpInputString = userInputText.text;

			if (IsThisColumnExample())
			{
				userInputText.text = userInputDigit.ToString() + tmpInputString; //changing order of inputed digits

				if (correctAnswerLength == 3 && userInputText.text.Length < 3)
					invisible.text = "<color=#DCDF71FF>?</color>" + "<color=#FFFFFF00>" + userInputText.text + "</color>";
				else
					invisible.text = "";
			}
			else
			{
				userInputText.text = tmpInputString + userInputDigit.ToString();

				if (correctAnswerLength == 3 && userInputText.text.Length < 3)
					invisible.text = "<color=#FFFFFF00>" + userInputText.text + "</color>" + "<color=#DCDF71FF>?</color>";
				else
					invisible.text = "";
			}

			FlipDigitsActivationToggle();
		}

		WobbleUsersInputDigit();
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
					{
						invisible.text = "<color=#DCDF71FF>?</color>" + "<color=#FFFFFF00>" + userInputText.text + "</color>";
					}

					FlipDigitsActivationToggle();
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
		flipDigitsToggle.interactable = false;

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


	void FlipDigitsActivationToggle()
	{
		if (correctAnswerLength == userInputText.text.Length)
		{
			flipDigitsToggle.interactable = true;
			this.GetComponent<StretchRect>().StretchRectTransformToMatchUserInput(flipDigitsToggle.gameObject.GetComponent<RectTransform>());
		}
		else
		{
			flipDigitsToggle.interactable = false;
		}
	}
}
