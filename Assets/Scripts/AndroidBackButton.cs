using UnityEngine;

public class AndroidBackButton : MonoBehaviour
{
	void Update()
	{
		#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.GetComponent<LoadLevel>().LoadScene(0);
		}
		#endif
	}
}
