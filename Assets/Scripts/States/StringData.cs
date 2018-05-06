using UnityEngine;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/States/String", fileName="String State", order = 42)]
public class StringData : ScriptableObject 
{
	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	public string Data;
}
