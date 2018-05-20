using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/ExampleTypesData/Toggles", fileName = "MultTableToggles")]
public class ExampleTogglesData : ScriptableObject
{

	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	public bool[] toggles;


}
