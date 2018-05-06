using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeHolder : MonoBehaviour
{
	public TimeData bestTime;
	public Text timerText;

	public void OnEnable()
	{
		timerText.text = bestTime.minutes.ToString().PadLeft(2, '0') + ":" + bestTime.seconds.ToString("F1").PadLeft(4, '0') + Lean.Localization.LeanLocalization.GetTranslationText("s");
	}
}
