using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class StopCountTo10Test : MonoBehaviour
{
	[Header("State Data")]
	public NonRangedStateData failsNum;
	public NonRangedStateData pooID;
	public StateData correctAnswersNum;
	public StateData finishCondition;
	public BoolData isLoadSuccessful;

	[Header ("Time data")]
	public GameObject recordsHolder;

	[Header ("Destination Places")]
	public Transform underground;
	public Transform topRight;
	public Transform undergroundTop;
	public Transform undergroundBottom;

	[Header("Objects to move")]
	public GameObject digits;
	public GameObject back;

	[Header ("Objects to show")]
	public GameObject moneyText;
	public GameObject victoryImage;
	public GameObject earnImage;
	public GameObject againButton;


	[Header("Objects to hide")]
	public GameObject usersInput;
	public GameObject mathExample;

	public IEnumerator LaunchFinalScoreProcesses()
	{
		Analytics.CustomEvent("Test_Finished", new Dictionary<string, object>
		{
			{"Level_ID",  "Level_" + this.GetComponent<ExampleGenerator>().exampleSwitch.Value},
			{"Correct_answers",  correctAnswersNum.Value},
			{"Wrong_answers", failsNum.Value },
			{"Currency", this.GetComponent<Money>().currencyTypes.Currencies[this.GetComponent<Money>().currencyID.Value].sign},
			{"Reward", this.GetComponent<Money>().moneyArray.CurrencyAmounts[this.GetComponent<Money>().currencyID.Value].RewardsArray[this.GetComponent<ExampleGenerator>().exampleSwitch.Value]}
		});

		StartCoroutine(this.GetComponent<Records>().LoadOrCreateRecords());

		StartCoroutine(this.GetComponent<Actions>().Move(digits, undergroundBottom, 0.2f));
		StartCoroutine(this.GetComponent<Actions>().Move(back, underground, 0.2f));

			//time to wait for last coin fall down
			yield return new WaitForSeconds(0.8f);
		Vector3 initialScale = new Vector3(1.5f, 1.5f, 1f);
		usersInput.SetActive (false);
		mathExample.SetActive (false);
		StartCoroutine(this.GetComponent<Actions>().Boom(victoryImage, initialScale));
			yield return new WaitForSeconds(0.2f);
		StartCoroutine(this.GetComponent<Actions>().Boom(earnImage, initialScale));
			yield return new WaitForSeconds(0.2f);
		this.GetComponent<Money>().PayAndPrint(0);
		StartCoroutine(this.GetComponent<Actions>().Boom(moneyText, initialScale));

		yield return StartCoroutine(DestroyPoosPayForCoins());
			yield return new WaitForSeconds(0.8f);
		recordsHolder.SetActive(true);
		yield return StartCoroutine(this.GetComponent<Actions>().Move(recordsHolder, topRight, 0.5f));

		if (isLoadSuccessful.toggle)
		{
			yield return StartCoroutine(this.GetComponent<Records>().ReplacePreviousRecordWithCurrentTime()); 
		}

		//wait for Records show off and for newRecordBadge show off
			yield return new WaitForSeconds(1f);

		this.GetComponent<Money>().SaveMoneyToFile(); 

		this.GetComponent<AudioSource>().Play();
		StartCoroutine(this.GetComponent<Actions>().Move(back, undergroundTop, 0.2f));
		againButton.SetActive(true);
	}


	/// <summary>
	/// Pay for every Coin if no Poo. In case of Poo - explode all poo with paired Coins
	/// </summary>
	/// <returns></returns>
	IEnumerator DestroyPoosPayForCoins ()
	{
		//Waiyt before count process starts
		yield return new WaitForSeconds(0.5f);                                     

		if (IsNoFails())
		{
			this.GetComponent<Coins>().CountCoins();
		}
		else
		{
			yield return StartCoroutine(PoosDestroyCoinsExceptLast());

			yield return new WaitForSeconds(0.5f);                                //Pause and count coins
			this.GetComponent<Coins>().CountCoins();
		}
	}


	IEnumerator PoosDestroyCoinsExceptLast()
	{
		for (int i = pooID.Value; i >= 0; i--)
		{
			if (i > finishCondition.Value)									//if there are more Poos than Coins
			{
				StartCoroutine(this.GetComponent<Poo>().DestroyPoo(i));
			}
			else
			{
				yield return new WaitForSeconds(0.3f);                      //..pause between explodes
				StartCoroutine(this.GetComponent<Poo>().DestroyPoo(i));
				if (correctAnswersNum.Value > 1)							// ">1" because I need to explode all coins except last one
					correctAnswersNum.ApplyChange(-1);
			}
		}
	}


	bool IsNoFails()
	{
		return failsNum.Value == 0;
	}
}

