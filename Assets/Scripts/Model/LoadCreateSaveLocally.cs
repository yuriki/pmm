using System.IO;
using System;
using UnityEngine;

public class LoadCreateSaveLocally : MonoBehaviour
{
	public MoneyArray moneyArray;
	public StateData currencyID;
	public StringData appPath;

	public BoolData firstLoad;
	
	[NonSerialized]
	public string money_FileName = "CurrencyAmount_ID";
	[NonSerialized]
	public string record_FileName = "_Record";
	[NonSerialized]
	public string pass_FileName = "Pass.txt";
	[NonSerialized]
	public string currencyID_FileName = "CurrencyID";
	[NonSerialized]
	public string fileExtension = ".json";


	void Awake()
	{
		if (firstLoad.toggle)
		{
			SetAppPath();

			currencyID.Value = 0; //$ is default currency. But if json file with other value exists, overwrite from file happens
			LoadOrCreateJSONLocally(currencyID_FileName, currencyID);

			ResetEveryCurrencyAmount();
			CreateOrLoadAmountOfEveryCurrency();

			firstLoad.toggle = false;
		}
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
		for (int i = 0; i < moneyArray.CurrencyAmounts.Length; i++)
		{
			moneyArray.CurrencyAmounts[i].Value = 0;
			moneyArray.CurrencyAmounts[i].PaidValue = 0;
			ResetRewardAmount(i);
		}
	}


	void ResetRewardAmount(int id)
	{
		for (int j = 0; j < moneyArray.CurrencyAmounts[id].RewardsArray.Length; j++)
		{
			if (j == 0) //reward for 10-9=? example
			{
				if (id == 4) //if Bitcoin
					moneyArray.CurrencyAmounts[id].RewardsArray[j] = 600;
				else
					moneyArray.CurrencyAmounts[id].RewardsArray[j] = 5;
			}
			else
			{
				if (id == 4) //if Bitcoin
					moneyArray.CurrencyAmounts[id].RewardsArray[j] = 2400;
				else
					moneyArray.CurrencyAmounts[id].RewardsArray[j] = 20;
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
		File.Delete(appPath.Data + pass_FileName);
	}
}
