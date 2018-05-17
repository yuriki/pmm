using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Yuriki's ScriptableObj assets/ExampleTypes/ExampleTypeData", fileName = "10")]
public class ExampleTypeData : ScriptableObject
{

	#if UNITY_EDITOR
		[Multiline]
		public string DeveloperDescription = "";
	#endif

	[Range (0,5)]
	public int exampleType;
	public string buttonName;
	public string shortButtonName;


}
