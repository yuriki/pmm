using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	public Text timerText;
	public BoolData isTestRunning;
	public TimeData time;

	public void resetTime()
	{
		time.seconds = 0;
		time.minutes = 0;
	}

	public IEnumerator StartTimer ()
	{
		while (isTestRunning.toggle)
		{
			//check for only positive values for manual mode through editor
			if (time.seconds < 0)
			{
				time.seconds = 0;
			}
			if (time.minutes < 0)
			{
				time.minutes = 0;
			}

			time.seconds += Time.deltaTime;

			UpdateTimer (time.seconds, time.minutes);
			if (time.seconds >= 59.99)
			{
				time.minutes ++;
				time.seconds = 0;
			}
			yield return null;
		}
	}


	void UpdateTimer (float sec, int min)
	{
		timerText.text = min.ToString().PadLeft(2,'0') + ":" + sec.ToString("F1").PadLeft(4,'0') + Lean.Localization.LeanLocalization.GetTranslationText("s");
	}
}
