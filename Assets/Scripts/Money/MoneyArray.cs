using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/Money Array", fileName = "Amounts", order = 35)]
public class MoneyArray : ScriptableObject
{
	public MoneyData[] CurrencyAmounts;
}
