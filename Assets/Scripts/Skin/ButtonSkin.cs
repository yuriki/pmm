using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
[RequireComponent (typeof(Image))]
public class ButtonSkin : Skin
{
	protected Button button;
	protected Image image;

	public override void Awake ()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();

		base.OnSkinUI ();
	}

	protected override void OnSkinUI ()
	{
		button.transition = buttonSkin.transitionType;
		button.targetGraphic = image;

		image.sprite = buttonSkin.buttonSprite;
		image.type = buttonSkin.spriteType;
		button.colors = buttonSkin.buttonColorState;

		//Use color depending of buttonType
		if (buttonType != null)
			image.color = buttonType.color;

	}
}
