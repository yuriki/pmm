using UnityEngine;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/States/Int", fileName="State", order = 41)]
public class StateData : ScriptableObject 
{
	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	[Range (0, 10)]
	public int Value;

	public void SetValue(int value)
	{
		Value = value;
	}

	public void ApplyChange (int amount)
	{
		Value += amount;
	}
}
