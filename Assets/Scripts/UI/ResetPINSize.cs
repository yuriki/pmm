using UnityEngine.UI;
using UnityEngine;

public class ResetPINSize : MonoBehaviour
{
	LayoutElement layout;
	int screenHeight;
	public Camera cam;

	private void Start()
	{
		screenHeight = cam.pixelHeight;
		layout = this.GetComponent<LayoutElement>();
		layout.preferredHeight = screenHeight *0.075f;
	}

}
