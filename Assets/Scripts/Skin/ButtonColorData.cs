using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/Button Type", fileName="T_", order = 33)]
public class ButtonColorData : ScriptableObject
{
	[Tooltip ("Color of Source Image sprite")]
	public Color color = new Color (1,1,1,1);
}
