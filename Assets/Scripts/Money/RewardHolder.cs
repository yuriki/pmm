using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardHolder : MonoBehaviour
{
	public MoneyArray moneyArray;
	public StateData currencyID;
	public Text rewardPlaceholderText;

	public void OnEnable()
	{
		float rewardAmountFloat = moneyArray.CurrencyAmounts[currencyID.Value].RewardsArray[0] * 0.01f;
		rewardPlaceholderText.text = rewardAmountFloat.ToString("F2").PadLeft(4, '0');
	}
}
