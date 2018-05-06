using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCorrectAnswer : MonoBehaviour 
{
	public BoolData isTestRunning;
	/// <summary>
	/// Drop coin, add it to array, generate new math example
	/// </summary>
	public void DoOnCorrectAnswer(StateData coinID)
	{
		//removing crossed wrong answer
		this.GetComponent<OnWrongAnswer>().crossHolder.SetActive(false);

		StartCoroutine (this.GetComponent<Coins>().DropCoin());
		StartCoroutine(this.GetComponent<Coins>().AddCoinToArray(coinID.Value));

		if (isTestRunning.toggle)
			this.GetComponent<ExampleGenerator>().NewExample();
	}
}
