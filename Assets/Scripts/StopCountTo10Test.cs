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

	[Header("Objects to move")]
	public GameObject digits;
	public GameObject back;

	[Header ("Objects to show")]
	public GameObject moneyText;
	public GameObject victoryImage;
	public GameObject earnImage;
	public GameObject againButton;
	public GameObject exitButton;

	[Header("Objects to hide")]
	public GameObject usersInput;
	public GameObject mathExample;

	public IEnumerator LaunchFinalScoreProcesses()
	{
		exitButton.SetActive(false);

#if UNITY_IOS || UNITY_ANDROID
		Analytics.CustomEvent("Test_Finished", new Dictionary<string, object>
		{
			{"Level_ID",  "Level_" + this.GetComponent<ExampleGenerator>().exampleSwitch.Value},
			{"Correct_answers",  correctAnswersNum.Value},
			{"Wrong_answers", failsNum.Value },
			{"Currency", this.GetComponent<Money>().currencyTypes.Currencies[this.GetComponent<Money>().currencyID.Value].sign},
			{"Reward", this.GetComponent<Money>().moneyArray.CurrencyAmounts[this.GetComponent<Money>().currencyID.Value].RewardsArray[this.GetComponent<ExampleGenerator>().exampleSwitch.Value]}
		}); 
#endif

		StartCoroutine(this.GetComponent<Records>().LoadOrCreateRecords());

		digits.GetComponent<RectTransform>().pivot = new Vector2(.5f, .5f);
		iTween.ScaleTo(digits, iTween.Hash("x", 1.1f, "y", 1.1f, "time", .05f));
		iTween.ScaleTo(digits, iTween.Hash("x", .01f, "y", .01f, "time", .2f, "easetype", "easeInCirc", "delay", .05f));
		yield return new WaitForSeconds(0.27f);
		digits.SetActive(false);

		yield return new WaitForSeconds(0.1f);
		iTween.MoveTo(back, iTween.Hash("x", underground.position.x, "y", underground.position.y, "time", .5f, "easetype", "easeInOutQuad"));

		//showing VICTORY! sign
		yield return new WaitForSeconds(0.5f);
		victoryImage.SetActive(true);
		iTween.PunchScale(victoryImage, iTween.Hash("x", 1.5f, "y", 1.5f, "time", .5f));
		usersInput.SetActive(false);
		mathExample.SetActive(false);


		//time to wait for last coin fall down
		yield return new WaitForSeconds(0.6f);

		//show money Text and label
		earnImage.SetActive(true);
		iTween.PunchScale(earnImage, iTween.Hash("x", 1.5f, "y", 1.5f, "time", .4f));
		yield return new WaitForSeconds(0.2f);
		this.GetComponent<Money>().PayAndPrint(0);
		moneyText.SetActive(true);
		iTween.PunchScale(moneyText, iTween.Hash("x", 1.5f, "y", 1.5f, "time", .4f));

		//Count coins
		yield return StartCoroutine(DestroyPoosPayForCoins());

		//wait for RecordsTable show off 
		yield return new WaitForSeconds(1.0f);
		recordsHolder.SetActive(true);
		iTween.MoveTo(recordsHolder, topRight.position, 0.2f);

		//wait for Timer fly to its place and newRecordBadge show off
		yield return new WaitForSeconds(0.4f);
		if (isLoadSuccessful.toggle)
		{
			yield return StartCoroutine(this.GetComponent<Records>().ReplacePreviousRecordWithCurrentTime()); 
		}

		//wait before Save money to file and show Again button
		yield return new WaitForSeconds(1.1f);

		this.GetComponent<Money>().SaveMoneyToFile(); //TODO save money in the begining of LaunchFinalScoreProcesses

		//Show Again button
		this.GetComponent<AudioSource>().Play();
		iTween.MoveTo(back, undergroundTop.position, .3f);
		againButton.SetActive(true);
		exitButton.SetActive(true);

	}


	/// <summary>
	/// Pay for every Coin if no Poo. In case of Poo - explode all poo with paired Coins
	/// </summary>
	/// <returns></returns>
	IEnumerator DestroyPoosPayForCoins ()
	{                                 

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

