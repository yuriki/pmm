using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/Currency Array", fileName="All Currencies", order = 35)]
public class CurrencyArray : ScriptableObject
{
	public CurrencyType [] Currencies;
}
