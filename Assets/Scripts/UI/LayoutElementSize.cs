using UnityEngine.UI;
using UnityEngine;

public class LayoutElementSize : MonoBehaviour
{
	LayoutElement layout;
	int screenHeight;
	public Camera cam;

	private void Start()
	{
		screenHeight = cam.pixelHeight;
		layout = this.GetComponent<LayoutElement>();
		layout.preferredHeight = screenHeight *0.15f;
	}

}
