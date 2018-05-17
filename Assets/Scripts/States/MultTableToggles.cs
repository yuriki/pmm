using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/Toggles/MultTableToggles", fileName = "MultTableToggles")]
public class MultTableToggles : ScriptableObject
{

	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	public bool[] toggles;


}
