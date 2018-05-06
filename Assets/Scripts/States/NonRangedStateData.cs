using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/States/NonRangedInt", fileName = "Non ranged State")]
public class NonRangedStateData : ScriptableObject
{
	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	public int Value;

	public void SetValue(int value)
	{
		Value = value;
	}

	public void ApplyChange(int amount)
	{
		Value += amount;
	}
}
