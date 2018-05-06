using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MakeScreenshotByMouseClick : MonoBehaviour
{
	public Camera cam;

	void Start()
	{
		ScreenCapture.CaptureScreenshot("Assets/JSON/Sreenshot_" + cam.pixelWidth + "x" + cam.pixelHeight + "_" + Lean.Localization.LeanLocalization.CurrentLanguage + "_" + SceneManager.GetActiveScene().name + ".png");
		StartCoroutine(ScreenshotWithDelay());
	}

	IEnumerator ScreenshotWithDelay()
	{
		yield return new WaitForSeconds(8);
		ScreenCapture.CaptureScreenshot("Assets/JSON/Sreenshot_" + cam.pixelWidth + "x" + cam.pixelHeight + "_" + Lean.Localization.LeanLocalization.CurrentLanguage + "_" + SceneManager.GetActiveScene().name + "_Delayed.png");
	}
}
