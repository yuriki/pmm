using UnityEngine.UI;
using UnityEngine;

public class LayoutElementSize : MonoBehaviour
{
	LayoutElement layout;
	int screenHeight;

	public Camera cam;
	public float multipliyer = 0.15f;
	public int spacing;

	private void Start()
	{
		screenHeight = cam.pixelHeight;
		layout = this.GetComponent<LayoutElement>();
		layout.preferredHeight = screenHeight*multipliyer + spacing;
	}

}
