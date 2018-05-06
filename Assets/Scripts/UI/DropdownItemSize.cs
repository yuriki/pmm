using UnityEngine;
using UnityEngine.UI;

public class DropdownItemSize : MonoBehaviour
{
	RectTransform trans;
	int screenHeight;
	public Camera cam;

	private void Start()
	{
		Canvas.ForceUpdateCanvases();

		screenHeight = cam.pixelHeight;
		trans = this.GetComponent<RectTransform>();
		Vector2 size = trans.sizeDelta;
		size.y =  screenHeight * 0.05f;
		trans.sizeDelta = size;

		
	}
}
