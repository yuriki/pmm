using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

		recordsHolder.transform.position = topLeft.position;
		recordsHolder.SetActive(false);

		timer.SetActive(false);

		startButton.SetActive(true);
		mathExample.SetActive (false);
		userInput.SetActive(false);

		instruction.SetActive(true);

		digits.SetActive(false);
		digits.transform.position = bottomLeft.position;
	}

	/// <summary>
	/// For button "Start_Button". Launching test and timer
	/// </summary>
	public void StartTest ()
	{
		digits.SetActive(true);
		StartCoroutine(this.GetComponent<Actions>().Move(digits, bottomMiddle, 0.5f));
		StartCoroutine(this.GetComponent<Actions>().Move(instruction, bottomRight, 0.5f));

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
