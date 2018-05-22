using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Records : MonoBehaviour
{
	public AudioClip floopAudio;

	[Header("States")]
	public StateData levelSwitch;
	public BoolData isLoadSuccessful;
	

	[Header("Time")]
	public TimeHolder[] timeHolderArray;
	public TimeData currentTimeData;

	[Header("Objects to hide/show")]
	public GameObject recordBadge;
	public GameObject timer;

	[Header("Destination Places")]
	public Transform[] places;

	LoadCreateSaveLocally LCSL;

	
	public IEnumerator LoadOrCreateRecords()
	{
		LCSL = this.GetComponent<LoadCreateSaveLocally>();
		for (int place = 0; place < timeHolderArray.Length; place++)
		{
			timeHolderArray[place].bestTime.minutes = 0;
			timeHolderArray[place].bestTime.seconds = 0;
			LCSL.LoadOrCreateJSONLocally("Example" + levelSwitch.Value + LCSL.record_FileName + place, timeHolderArray[place].bestTime);
		}
		yield return null;

		isLoadSuccessful.toggle = true;
	}

	public IEnumerator ReplacePreviousRecordWithCurrentTime()
	{
		float currentTime = currentTimeData.minutes * 60 + currentTimeData.seconds;

		//Comparing all records from records table with current time
		for (int place = 0; place < timeHolderArray.Length; place++)
		{
			float currentRecordTime = timeHolderArray[place].bestTime.minutes * 60 + timeHolderArray[place].bestTime.seconds;
			//Checking if this is fresh (first) record to table of records
			if (currentRecordTime == 0)
			{
				SaveNewRecord(place);

				//if this is first place show "NewRecord" badge
				if (place == 0)
					StartCoroutine(ShowRecordBadge());

				MoveTimerToPlace(place);
				//stop FOR-loop to not overwrite all records but only current one
				break;
			}
			//If this is not fresh record I compare new and old time
			else if (currentTime < currentRecordTime)
			{
				ShiftPreviousRecordDown(place);

				SaveNewRecord(place);

				//if this is first place show "NewRecord" badge
				if (place == 0)
					StartCoroutine(ShowRecordBadge());

				MoveTimerToPlace(place);
				//stop FOR-loop to not overwrite all records but only current one
				break;
			}
			else if (IsThisLastPlace(place))
			{
				MoveTimerToPlace(3);
			}
		}

		yield return null;
	}


	IEnumerator ShowRecordBadge()
	{
		yield return new WaitForSeconds(0.7f);
		recordBadge.SetActive(true);
		iTween.PunchScale(recordBadge, new Vector3(2, 2, 1), .7f);
		timer.GetComponent<AudioSource>().PlayOneShot(floopAudio);
	}

	bool IsThisLastPlace(int place)
	{
		return place == timeHolderArray.Length - 1;
	}


	void SaveNewRecord(int place)
	{
		timeHolderArray[place].bestTime.minutes = currentTimeData.minutes;
		timeHolderArray[place].bestTime.seconds = currentTimeData.seconds;
		
		LCSL.SaveJSONLocally("Example" + levelSwitch.Value + LCSL.record_FileName + place, timeHolderArray[place].bestTime);
		timeHolderArray[place].OnEnable();
	}


	void MoveTimerToPlace(int place)
	{
		timer.GetComponent<AudioSource>().Play();
		iTween.PunchScale(timer, iTween.Hash("x", 2f, "y", 2f, "time", 1.2f));
		iTween.MoveTo(timer, iTween.Hash("x", places[place].position.x, "y", places[place].position.y, "time", .3f, "easetype", "linear"));
	}


	void ShiftPreviousRecordDown(int place)
	{
		//I'm starting from last record in records table because I can freely throw away last record
		for (int j = timeHolderArray.Length - 1; j > place; j--)
		{
			timeHolderArray[j].bestTime.minutes = timeHolderArray[j - 1].bestTime.minutes;
			timeHolderArray[j].bestTime.seconds = timeHolderArray[j - 1].bestTime.seconds;
			LCSL.SaveJSONLocally("Example" + levelSwitch.Value + LCSL.record_FileName + j, timeHolderArray[j].bestTime);
			timeHolderArray[j].OnEnable();
		}
	}
}
