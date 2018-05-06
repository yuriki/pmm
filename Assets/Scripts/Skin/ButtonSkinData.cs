using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/Skin for buttons", fileName="New Skin", order = 32)]
public class ButtonSkinData : ScriptableObject
{
	public Sprite buttonSprite;
	public ColorBlock buttonColorState;
	public Image.Type spriteType;
	public Button.Transition transitionType;
}
