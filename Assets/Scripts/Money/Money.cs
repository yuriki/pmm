using System.Globalization;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Money : MonoBehaviour
{
	public Text moneyText;
	public StateData currencyID;
	public CurrencyArray currencyTypes;
	public MoneyArray moneyArray;


	void Start()
	{
		//if main menu show money on Pig
		if (SceneManager.GetActiveScene().buildIndex == 0)
			PayAndPrint(0);
	}

	/// <summary>
	/// Pay and show money text
	/// </summary>
	/// <param name="amount">How much current currency to pay</param>
	public void PayAndPrint(int amount)
	{
		moneyArray.CurrencyAmounts[currencyID.Value].Value += amount;
		PrintMoney(moneyArray.CurrencyAmounts[currencyID.Value].Value, moneyText);
	}


	/// <summary>
	/// Pay + PaidOut and show money text
	/// </summary>
	/// <param name="amount">How much current currency to pay</param>
	/// <param name="isPaidOut">Do I need to change also PaidOut amount</param>
	public void PayAndPrint(int amount, bool isPaidOut)
	{
		moneyArray.CurrencyAmounts[currencyID.Value].Value += amount;
		PrintMoney(moneyArray.CurrencyAmounts[currencyID.Value].Value, moneyText); //showing current amount of money on pig

		if (isPaidOut)
		{
			moneyArray.CurrencyAmounts[currencyID.Value].PaidValue -= amount;
		}
	}


	/// <summary>
	/// Save monye amount of current currency to json file
	/// </summary>
	public void SaveMoneyToFile()
	{
		LoadCreateSaveLocally LCSL = this.GetComponent<LoadCreateSaveLocally>();
		LCSL.SaveJSONLocally(LCSL.money_FileName + currencyID.Value, moneyArray.CurrencyAmounts[currencyID.Value]);
	}


	public void PrintMoney(int moneySum, Text outputText)
	{
		//converting int to float to print number with decimal point 
		float moneyFloat = (float)moneySum;
		//dividing by 100 to have cents
		moneyFloat *= 0.01f;
		var f = new NumberFormatInfo { NumberGroupSeparator = Lean.Localization.LeanLocalization.GetTranslationText("separator") }; //thousands separator
		if (IsDollar() || IsEuro() || IsBitcoin() || IsPound() || IsPoint())
		{
			if (IsBitcoin() || IsPoint())
			{
				outputText.text = currencyTypes.Currencies[currencyID.Value].sign + moneySum.ToString("N0", f);
			}
			else
			{
				outputText.text = currencyTypes.Currencies[currencyID.Value].sign + moneyFloat.ToString("N2", f);
			}
		}
		else
		{
			outputText.text = moneyFloat.ToString("N2", f) + " " + currencyTypes.Currencies[currencyID.Value].sign;
		}
	}


	bool IsBitcoin()
	{
		return currencyID.Value == 4;
	}

	bool IsDollar()
	{
		return currencyID.Value == 0;
	}

	bool IsEuro()
	{
		return currencyID.Value == 1;
	}

	bool IsPound()
	{
		return currencyID.Value == 5;
	}

	bool IsPoint()
	{
		return currencyID.Value == 6;
	}
}
