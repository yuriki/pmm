using UnityEngine;

public class IphoneXAdaptation : MonoBehaviour
{
	public RectTransform dgtBtn;
	public RectTransform exitBtn;
	public GameObject timer;
	public Camera cam;

	//Rect safeArea;

	void Start()
	{
		//TODO make properly safe area for iPHONE X
		//safeArea = Screen.safeArea;

		if (cam.pixelHeight == 2436 && cam.pixelWidth == 1125)
		{
			Vector2 ofs = dgtBtn.offsetMin;
			ofs.y = 25;
			dgtBtn.offsetMin = ofs;

			ofs = dgtBtn.offsetMax;
			ofs.y = -25;
			dgtBtn.offsetMax = ofs;

			ofs = exitBtn.offsetMin;
			ofs.y -= 95;
			exitBtn.offsetMin = ofs;

			ofs = exitBtn.offsetMax;
			ofs.y -= 95;
			exitBtn.offsetMax = ofs;
		}		
	}
}
