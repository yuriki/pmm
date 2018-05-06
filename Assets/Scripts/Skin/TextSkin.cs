using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Text))]
public class TextSkin : Skin
{
	protected Text text;
	

	public override void Awake ()
	{
		text = GetComponent<Text>();

		base.OnSkinUI ();
	}


	protected override void OnSkinUI ()
	{
		//Use color depending of buttonType
		if (buttonType != null)
			text.color = buttonType.color;
	}
}
