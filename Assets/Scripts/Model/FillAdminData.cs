using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class FillAdminData : MonoBehaviour
{
	public CurrencyArray currencyArray;
	Dropdown.OptionData dataOption;
	public Dropdown currenciesDropdown;
	public StateData currencyID;
	public MoneyArray moneyArray;


	int userInputMoneyValue;
	bool isChanged = false;
	int tmpCurrencyID;

	[Space (10)]
	public InputField[] rewardFields;
	public InputField payOffField;
	public Text payOffFieldPlaceholder;
	public Text alreadyPaid;
	public Text label;

	float rewardAmountFloat;


	private void Start()
	{
		tmpCurrencyID = currencyID.Value;

		AddCurrenciesToDropdownList();
		FillRewardFields();
		ShowEarnedAndPaidMoney();

		Analytics.CustomEvent("Settings_Opened", new Dictionary<string, object>
		{
			{"Language", Lean.Localization.LeanLocalization.CurrentLanguage}
		});
	}


	public void SettingsClosed()
	{
		Analytics.CustomEvent("Settings_Closed", new Dictionary<string, object>
		{
			{"Currency", this.GetComponent<Money>().currencyTypes.Currencies[this.GetComponent<Money>().currencyID.Value].sign}
		});
	}


	void ShowEarnedAndPaidMoney()
	{
		this.GetComponent<Money>().PayAndPrint(0);

		this.GetComponent<Money>().PrintMoney(moneyArray.CurrencyAmounts[currencyID.Value].PaidValue, alreadyPaid);
	}


	void FillRewardFields()
	{
		var f = new NumberFormatInfo { NumberGroupSeparator = Lean.Localization.LeanLocalization.GetTranslationText("separator") }; //thousands separator

		for (int i = 0; i <rewardFields.Length; i++)
		{
			Text placeholderText = rewardFields[i].placeholder as Text;
			if (IsBitcoin())
			{
				placeholderText.text = moneyArray.CurrencyAmounts[currencyID.Value].RewardsArray[i].ToString("N0", f);
			}
			else
			{
				rewardAmountFloat = moneyArray.CurrencyAmounts[currencyID.Value].RewardsArray[i] * 0.01f;
				placeholderText.text = rewardAmountFloat.ToString("N2", f).PadLeft(4, '0');
			}
		}
	}


	public void RefillRewardsAfterChooseDropdownOption(int optionID)
	{
		currencyID.Value = optionID;
		FillRewardFields();
		SaveCurrencyIDToFile();
	}


	void AddCurrenciesToDropdownList()
	{
		foreach (var currency in currencyArray.Currencies)
		{
			dataOption = new Dropdown.OptionData { text = Lean.Localization.LeanLocalization.GetTranslationText(currency.currencyName) + " (" + currency.sign + ")" };

			currenciesDropdown.options.Add(dataOption);
		}
		//Setting current currency to be default value in dropdown list
		currenciesDropdown.value = currencyID.Value;

		if (currencyID.Value == 0)
		{
			label.text = Lean.Localization.LeanLocalization.GetTranslationText(currencyArray.Currencies[0].currencyName) + " (" + currencyArray.Currencies[0].sign + ")";
		}
	}



	//public void SaveParameters ()
	//{
	//	SaveCurrencyIDToFile();
	//	SaveAllRewardsToFile();
	//}


	void SaveCurrencyIDToFile()
	{
		if (currencyID.Value != tmpCurrencyID)
		{
			tmpCurrencyID = currencyID.Value;
			LoadCreateSaveLocally LCSL = this.GetComponent<LoadCreateSaveLocally>();
			LCSL.SaveJSONLocally(LCSL.currencyID_FileName, currencyID);
			ShowEarnedAndPaidMoney();

			//ChangeLaguage();
		}
	}


	void ChangeLaguage()
	{
		if (currencyID.Value == 2)
		{
			Lean.Localization.LeanLocalization.CurrentLanguage = "Russian"; // RUS
		}
		else if (currencyID.Value == 3)
		{
			Lean.Localization.LeanLocalization.CurrentLanguage = "Ukrainian"; //UA
		}
		else
		{
			Lean.Localization.LeanLocalization.CurrentLanguage = "English"; //ENG
		}
	}



	public void SaveAllRewardsToFile()
	{
		for (int i = 0; i < rewardFields.Length; i++)
		{
			if (rewardFields[i].text != "")
			{
				//float rewardFloat = float.Parse(rewardFields[i].text, CultureInfo.InvariantCulture.NumberFormat);
				//int rewardInt = Mathf.FloorToInt(rewardFloat * 100);
				moneyArray.CurrencyAmounts[currencyID.Value].RewardsArray[i] = ConvertStringToInt(rewardFields[i].text);
				rewardFields[i].text = "";
				isChanged = true;
			}
		}

		if (isChanged)
		{
			this.GetComponent<Money>().SaveMoneyToFile();
			FillRewardFields();
			isChanged = false;
		}
	}


	int ConvertStringToInt(string stringWithFloatValue)
	{
		if (IsBitcoin())
		{
			return int.Parse(stringWithFloatValue);
		}
		else
		{
			float floatValue = float.Parse(stringWithFloatValue, CultureInfo.InvariantCulture.NumberFormat);
			return Mathf.FloorToInt(floatValue * 100);
		}
	}


	public void ReadUserInput (string usersInput)
	{
		if (usersInput!="")
		{
			userInputMoneyValue = ConvertStringToInt(usersInput);
			
			//userInputMoneyValue = int.Parse(usersInput);
			//userInputMoneyValue = userInputMoneyValue * 100; //converting dollars to cents 
		}
	}


	bool IsBitcoin()
	{
		return currencyID.Value == 4;
	}

	public void PayOFF ()
	{
		this.GetComponent<Money>().PayAndPrint(-userInputMoneyValue, true);
		this.GetComponent<Money>().PrintMoney(moneyArray.CurrencyAmounts[currencyID.Value].PaidValue, alreadyPaid/*, true*/);

		this.GetComponent<Money>().SaveMoneyToFile();
		payOffFieldPlaceholder.text = Lean.Localization.LeanLocalization.GetTranslationText("Paid");
		payOffField.text = "";

		Analytics.CustomEvent("Cash_Paid", new Dictionary<string, object>
		{
			{"Currency", this.GetComponent<Money>().currencyTypes.Currencies[this.GetComponent<Money>().currencyID.Value].sign},
			{"Paid", userInputMoneyValue},
			{"PaidForAllTime", this.GetComponent<Money>().moneyArray.CurrencyAmounts[this.GetComponent<Money>().currencyID.Value].PaidValue}
		});

		userInputMoneyValue = 0;
	}
}
