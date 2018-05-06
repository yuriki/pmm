using UnityEngine;
using UnityEngine.UI;

public class Check : MonoBehaviour 
{
	[Tooltip ("Text object showing user's input")]
	public Text usersInputText;

	[Header("States")]
	public StateData correctAnswersNum;
	public NonRangedStateData wrongAnswersNum;
	public NonRangedStateData correctAnswer;

	int typedDigit;

	public void CheckResult () 
	{
		//getting user's inputed digit from Text object
		typedDigit = int.Parse (usersInputText.text);

		if (IsCorrectAnswer())
			correctAnswersNum.ApplyChange(1);
		else
			wrongAnswersNum.ApplyChange(1);
		
		//reset user's input
		typedDigit = -1;
	}

	bool IsCorrectAnswer()
	{
		return correctAnswer.Value == typedDigit;
	}
}
