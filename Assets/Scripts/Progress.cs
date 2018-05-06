using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Progress : MonoBehaviour 
{
	public StateData finishCondition;
	public BoolData isTestRunning;

	[Space]
	[Header ("ON CORRECT")]
	public StateData correctAnswersNum;
	[Tooltip ("Temporary ID of current coin. It'll be passed to other scripts to manipulate coins")]
	public StateData coinID;

	[Space]
	public UnityEvent onCorrectAnswer;
    public UnityEvent destroyCoin;

	[Space]
	[Header("ON WRONG")]
	public NonRangedStateData wrongAnswersNum;
	public NonRangedStateData pooID;
	[Space]
	public UnityEvent onWrongAnswer;
	public UnityEvent onDestroyPoo;

	int tmpCorrectValue;
	int tmpWrongValue;

	void Update ()
	{
		//limiting number of correct answers by not getting exceed finish condition value
		if (correctAnswersNum.Value > finishCondition.Value)
			correctAnswersNum.Value = finishCondition.Value;

		//checking for test FINISH
		if (isTestRunning.toggle && correctAnswersNum.Value >= finishCondition.Value)
		{
			isTestRunning.Off();
			StartCoroutine(this.GetComponent<StopCountTo10Test>().LaunchFinalScoreProcesses());
		}

		//settimg minimum number of wrong answers to zero
		if (wrongAnswersNum.Value < 0)
			wrongAnswersNum.Value = 0;

		//if number of CORRECT answers changed..
		if (correctAnswersNum.Value > tmpCorrectValue) //..to higher quantity
		{
			for (int i = 0; i < correctAnswersNum.Value - tmpCorrectValue; i++)
			{
				//calculating temporary id of current coin for case where user increases correctAnswersNum not by value=1 but by random value
				coinID.Value = i + tmpCorrectValue;

				onCorrectAnswer.Invoke();
			}
			tmpCorrectValue = correctAnswersNum.Value;
		}
		else if (correctAnswersNum.Value < tmpCorrectValue) //..to lower quantity
		{
			for (int i = 0; i < tmpCorrectValue - correctAnswersNum.Value; i++)
			{
				//calculating temporary id of current coin for case where user deletes correctAnswersNum not by value=1 but by random value
				coinID.Value = tmpCorrectValue - i - 1;

				destroyCoin.Invoke();
			}
			tmpCorrectValue = correctAnswersNum.Value;
		}

		//if number of WRONG answers changed..
		//tmpWrongValue = OnValueChanged(wrongAnswersNum.Value, tmpWrongValue, pooID.Value, onWrongAnswer, onDestroyPoo);
		if (wrongAnswersNum.Value > tmpWrongValue) //..to higher quantity
		{
			for (int i = 0; i < wrongAnswersNum.Value - tmpWrongValue; i++)
			{
				//calculating temporary ID of current coin for case where user increases correctAnswersNum not by value=1 but by random value
				pooID.Value = i + tmpWrongValue;

				onWrongAnswer.Invoke();
			}
			tmpWrongValue = wrongAnswersNum.Value;
		}
		else if (wrongAnswersNum.Value < tmpWrongValue) //..to lower quantity
		{
			for (int i = 0; i < tmpWrongValue - wrongAnswersNum.Value; i++)
			{
				//calculating temporary ID of current coin for case where user deletes correctAnswersNum not by value=1 but by random value
				pooID.Value = tmpWrongValue - i - 1;

				onDestroyPoo.Invoke();
			}
			tmpWrongValue = wrongAnswersNum.Value;
		}
	}


	//TODO do refarctoring of IF statements above
	
}
