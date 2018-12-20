using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoWriteReview : MonoBehaviour
{
	string androidUrl = "market://details?id=com.yuriki3d.PocketMoneyMath";
	string iosUrl = "itms-apps://itunes.apple.com/app/idcom.3dyuriki.PocketMoneyMath?action=write-review&mt=8";

	public void RateApp()
	{
		#if UNITY_ANDROID
			Application.OpenURL(androidUrl);
		#endif

		#if UNITY_IOS
			Application.OpenURL(iosUrl);
		#endif
	}
}
