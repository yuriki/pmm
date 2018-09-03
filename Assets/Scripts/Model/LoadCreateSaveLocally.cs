using System.IO;
using System;
using UnityEngine;

public class LoadCreateSaveLocally : MonoBehaviour
{
	public MoneyArray moneyArray;
	public StateData currencyID;
	public StringData appPath;
	public BoolData isHardExamplesOnly;

	public BoolData firstLoad;
	
	[NonSerialized]
	public string money_FileName = "CurrencyAmount_ID";
	[NonSerialized]
	public string record_FileName = "_Record";
	[NonSerialized]
	public string passFileNamePlusExt = "Pass.txt";
	[NonSerialized]
	public string currencyID_FileName = "CurrencyID";
	[NonSerialized]
	public string disableSimpleExamples_FileName = "DisSimEx";
	[NonSerialized]
	public string fileExtension = ".json";


	void Awake()
	{
		if (firstLoad.toggle)
		{
			SetAppPath();

			SetDefaultCurrency();
			LoadOrCreateJSONLocally(currencyID_FileName, currencyID);

			ResetEveryCurrencyAmount();
			CreateOrLoadAmountOfEveryCurrency();

			isHardExamplesOnly.toggle = false; //for the first time app shows all examples
			LoadOrCreateJSONLocally(disableSimpleExamples_FileName, isHardExamplesOnly);

			firstLoad.toggle = false;
		}
	}


	void SetDefaultCurrency()
	{
		currencyID.Value = 6; //Point is default currency. But if json file with other value exists, overwrite from file happens
	}


	void CreateOrLoadAmountOfEveryCurrency()
	{
		for (int i = 0; i < moneyArray.CurrencyAmounts.Length; i++)
		{
			LoadOrCreateJSONLocally(money_FileName + i, moneyArray.CurrencyAmounts[i]);
		}
	}


	void ResetEveryCurrencyAmount()
	{
		for (int currentCurrencyID = 0; currentCurrencyID < moneyArray.CurrencyAmounts.Length; currentCurrencyID++)
		{
			moneyArray.CurrencyAmounts[currentCurrencyID].Value = 0;
			moneyArray.CurrencyAmounts[currentCurrencyID].PaidValue = 0;
			ResetRewardAmount(currentCurrencyID);
		}
	}


	void ResetRewardAmount(int currentCurrency)
	{
		for (int exampleTypeID = 0; exampleTypeID < moneyArray.CurrencyAmounts[currentCurrency].RewardsArray.Length; exampleTypeID++)
		{
			if (exampleTypeID == 0) //reward for 10-9=? example
			{
				if (currentCurrency == 4) //if Bitcoin
					moneyArray.CurrencyAmounts[currentCurrency].RewardsArray[exampleTypeID] = 600;
				else if (currentCurrency == 6) //if Point
					moneyArray.CurrencyAmounts[currentCurrency].RewardsArray[exampleTypeID] = 1;
				else
					moneyArray.CurrencyAmounts[currentCurrency].RewardsArray[exampleTypeID] = 5;
			}
			else
			{
				if (currentCurrency == 4) //if Bitcoin
					moneyArray.CurrencyAmounts[currentCurrency].RewardsArray[exampleTypeID] = 2400;
				else if (currentCurrency == 6) //if Point
					moneyArray.CurrencyAmounts[currentCurrency].RewardsArray[exampleTypeID] = 2;
				else
					moneyArray.CurrencyAmounts[currentCurrency].RewardsArray[exampleTypeID] = 20;
			}
		}
	}


	void SetAppPath()
	{
		#if UNITY_IOS || UNITY_ANDROID
			appPath.Data = Application.persistentDataPath + "/";
			appPath.Data = appPath.Data.Replace ("\\", "/");
		#endif

		#if UNITY_EDITOR
			appPath.Data = "Assets/JSON/";
		#endif
	}


	public void LoadOrCreateJSONLocally(string fileName, UnityEngine.Object obj)
	{
		if (File.Exists(appPath.Data + fileName + fileExtension))
		{
			LoadJSONFromLocal(fileName, obj);
		}
		else
		{
			SaveJSONLocally(fileName, obj);
		}
	}


	//TODO add check for correct load from file
	//I can do this by using coroutine:
	//IEnumerator LoadJSONFromLocalFile(string fileName, UnityEngine.Object obj)
	//{
	//	String file;
	//	if (File.Exists(appPath.Data + fileName + fileExtension))
	//	{
	//		yield return file = File.ReadAllText(appPath.Data + fileName + fileExtension);
	//		JsonUtility.FromJsonOverwrite(file, obj);
	//	}
	//	else
	//	{
	//		Debug.LogError("File " + appPath.Data + fileName + fileExtension + " doesn't exist");
	//	}
	//}

	public void LoadJSONFromLocal(string fileName, UnityEngine.Object obj)
	{

		if (File.Exists(appPath.Data + fileName + fileExtension))
		{
			JsonUtility.FromJsonOverwrite(File.ReadAllText(appPath.Data + fileName + fileExtension), obj); 
		}
		else
		{
			Debug.Log("Can't read file: " + appPath.Data + fileName + fileExtension);
		}
	}


	public void SaveJSONLocally(string fileName, UnityEngine.Object obj)
	{
		File.WriteAllText(appPath.Data + fileName + fileExtension, JsonUtility.ToJson(obj));
	}


	public void DeletePasswordFile()
	{
		File.Delete(appPath.Data + passFileNamePlusExt);
	}
}
