using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MakeScreenshotByMouseClick : MonoBehaviour
{
	public Camera mainCamera;

	int counter = 1;

	//void Start()
	//{
	//	ScreenCapture.CaptureScreenshot("Assets/JSON/Sreenshot_" + cam.pixelWidth + "x" + cam.pixelHeight + "_" + Lean.Localization.LeanLocalization.CurrentLanguage + "_" + SceneManager.GetActiveScene().name + ".png");
	//	//StartCoroutine(ScreenshotWithDelay());
	//}


	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			ScreenCapture.CaptureScreenshot("Assets/Screenshots/Sreenshot" + counter.ToString("00") + "_" + mainCamera.pixelWidth + "x" + mainCamera.pixelHeight + "_" + Lean.Localization.LeanLocalization.CurrentLanguage + "_SceneID"+ SceneManager.GetActiveScene().name + ".png");
			counter++;
		}
	}


	//IEnumerator ScreenshotWithDelay()
	//{
	//	yield return new WaitForSeconds(8);
	//	ScreenCapture.CaptureScreenshot("Assets/JSON/Sreenshot_" + cam.pixelWidth + "x" + cam.pixelHeight + "_" + Lean.Localization.LeanLocalization.CurrentLanguage + "_" + SceneManager.GetActiveScene().name + "_Delayed.png");
	//}
}
