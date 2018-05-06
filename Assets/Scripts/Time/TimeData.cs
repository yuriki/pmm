using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/Time", fileName = "Time", order = 46)]
public class TimeData : ScriptableObject
{
	#if UNITY_EDITOR
		[Multiline]
		public string DevDescription = "";
	#endif

	public float seconds;
	public int minutes;
}
