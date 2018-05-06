using UnityEngine;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/States/Bool", fileName="Bool State", order = 42)]
public class BoolData : ScriptableObject 
{
	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	public bool toggle;

	public void On()
	{
		toggle = true;
	}
		
	public void Off ()
	{
		toggle = false;
	}
}
