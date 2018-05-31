using UnityEngine;

public class AndroidBackButtonQuit : MonoBehaviour
{
	void Update()
	{
		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		#endif
	}
}
