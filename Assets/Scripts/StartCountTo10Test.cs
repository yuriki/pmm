using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;

public class StartCountTo10Test : MonoBehaviour 
{
	[Header ("Time")]
	public BoolData isTestRunning;
	public GameObject timer;

	[Header ("Objects to move")]
	public GameObject digits;
	public GameObject instruction;
	public GameObject recordsHolder;

	[Header("Objects to hide/show")]
	public GameObject startButton;
	public GameObject mathExample;
	public GameObject userInput;
	public GameObject questionMarkGreen;

	[Header ("Destination Places")]
	public Transform bottomMiddle;
	public Transform bottomRight;
	public Transform topLeft;
	public Transform bottomLeft;


	[Header ("Progress Data")]
	public StateData correctAnswersNum;

	[Tooltip ("Set to 0 number of correct answers at the beginning of test")]
	public bool resetCorrectAnswersNum;

	[Space]
	public NonRangedStateData WrongAnswersNum;

	[Tooltip("Set to 0 number of wrong answers at the beginning of test")]
	public bool resetWrongAnswersNum;

	//Reseting mumber of correct and wrong answers if I need this
	void Start()
	{
		if (resetCorrectAnswersNum)
			correctAnswersNum.Value = 0;

		if (resetWrongAnswersNum)
			WrongAnswersNum.Value = 0;

		//hide unneeded
		recordsHolder.transform.position = topLeft.position;
		recordsHolder.SetActive(false);

		timer.SetActive(false);
		mathExample.SetActive (false);
		userInput.SetActive(false);
		
		digits.SetActive(false);
		digits.transform.position = bottomLeft.position;

		//show needed
		startButton.SetActive(true);
		instruction.SetActive(true);
	}

	/// <summary>
	/// For button "Start_Button". Launching test and timer
	/// </summary>
	public void StartTest ()
	{
#if UNITY_IOS || UNITY_ANDROID
		Analytics.CustomEvent("Test_Started", new Dictionary<string, object>
		{
			{"Level_ID",  "Level_" + this.GetComponent<ExampleGenerator>().exampleSwitch.Value}
		});
#endif

		digits.SetActive(true);
		iTween.MoveTo(digits, bottomMiddle.position, .3f);
		iTween.MoveTo(instruction, bottomRight.position, .8f);

		userInput.SetActive(true);

		this.GetComponent<ExampleGenerator>().NewExample();
		mathExample.SetActive(true);
		iTween.PunchScale(mathExample, new Vector3(1.5f, 1.5f, 1), .3f);
		iTween.PunchScale(questionMarkGreen, new Vector3(1.5f, 1.5f, 1), .3f);

		isTestRunning.On();

		//Starting Timer
		timer.GetComponentInChildren<Timer>().resetTime();
		timer.SetActive(true);
		StartCoroutine (timer.GetComponentInChildren<Timer>().StartTimer());

	}

}
