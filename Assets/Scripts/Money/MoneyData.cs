using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/States/MoneyData", fileName = "Amount_")]
public class MoneyData : ScriptableObject
{
	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	public int Value;
	public int PaidValue;

	[Space]
	public int[] RewardsArray;
}
