using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExampleGenerator : MonoBehaviour 
{
	public float offset;
	[Header("States")]
	public StateData exampleSwitch;
	public NonRangedStateData correctAnswer;
	public MultTableToggles toggles10;

	[Header("Text")]
	public Text inputResultText;
	public Text mathExamp;
	public Text invisible;
	public Text questionMarkRed;
	public GameObject questionMark;
	public RectTransform cross;

	[Header("Destination places")]
	public Transform userBottom;
	public Transform userTopLeft;

	int generated; 
	int tmpGenerated;

	int generated2; 
	int tmpGenerated2;

	int randomToggle;
	int signInt;
	string signStr;

	// counters to help get rid of infinite while-loop
	int i;
	int j;

	bool isFirstTimeWobble = true;
	bool isThisSecondAnswer10InRow;


	public void NewExample ()
	{
		inputResultText.text = "";
		questionMark.GetComponent<Text>().text = "?";

		if (exampleSwitch.Value == 0)
		{
			GenerateCountTo10Example();
		}
		else if (exampleSwitch.Value == 1)
		{
			GenerateMultiplicationTableExample();
		}
		else if (exampleSwitch.Value == 2)
		{
			GenerateColumnExample();
			ArrangeColumnExample();
		}


		if (isFirstTimeWobble)
		{
			StartCoroutine(WaitAndWobbleQuestionMark(3f));
			isFirstTimeWobble = false;
		}
	}


	void MoveQuestionMarkInside()
	{
		inputResultText.transform.position = userTopLeft.position;
	}

	void ArrangeColumnExample()
	{
		inputResultText.transform.position = userBottom.position;
		inputResultText.alignment = TextAnchor.MiddleRight;
		questionMarkRed.alignment = TextAnchor.MiddleRight;
		invisible.alignment = TextAnchor.MiddleRight;

		//changing pivot position of red cross to print it above first digit
		Vector2 crossPivot = cross.pivot;
		crossPivot.x = 1f;
		cross.pivot = crossPivot;

		//Vector2 ofs = cross.offsetMin;
		//ofs.x = 133;
		//cross.offsetMin = ofs;

		questionMark.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
	}


	IEnumerator WaitAndWobbleQuestionMark(float sec)
	{
		yield return new WaitForSeconds(sec);

		iTween.ScaleFrom(questionMark, iTween.Hash(
			"scale", new Vector3(1f, 0.5f, 1f),
			"easetype", "easeInQuad",
			"time", 0.4f));
		iTween.PunchPosition(questionMark, iTween.Hash(
			"amount", new Vector3(0f, 0.5f, 0f),
			"time", 0.9f,
			"delay", 0.2f));
	}


	void GenerateColumnExample()
	{
		// I'm making next generated value different from previous one (tmpGenerated)
		while (tmpGenerated == generated)
			generated = UnityEngine.Random.Range(10, 801);
		tmpGenerated = generated;

		while (tmpGenerated2 == generated2)
			generated2 = UnityEngine.Random.Range(1, 200);
		tmpGenerated2 = generated2;

		mathExamp.text = generated.ToString() + "\n" + "+        \n" + generated2.ToString() + "\n" + "-------";
		correctAnswer.Value = generated + generated2;
	}


	void GenerateCountTo10Example ()
	{
		//randomly choosing subtype of example based on user activated toggles (in TwoLevelsButton)
		randomToggle = UnityEngine.Random.Range(0, 3);
		while (!toggles10.toggles[randomToggle])
		{
			if (randomToggle != 2)
			{
				randomToggle++;
			}
			else
			{
				randomToggle = 0;
			}
		}
		

		signInt = ChooseSign();
		if (signInt == -1)
		{
			signStr = "-";
		}
		else
		{
			signStr = "+";
		}

		// tmpGenerated = generated = 0 after first initialization
		// I'm making next generated value different from previous one (different from tmpGenerated)
		while (tmpGenerated == generated)
		{
			generated = UnityEngine.Random.Range(1, 10);
		}
		tmpGenerated = generated;

		generated2 = UnityEngine.Random.Range(1, 10);
		i = 1;
		j = 1;
		//IMPORTANT I can use expression "==0" here only if I regenerate variable "generated". Oterwise I have infinite loop
		while ((generated + signInt * generated2) <= 0 || (generated + signInt * generated2) > 10 )
		{
			if ((generated + signInt * generated2) == 0)
			{
				j++;
				if (j > 3)
				{
					j = 1;
					break;
				}
			}

			generated2 = UnityEngine.Random.Range(1, 10);
			i++;
			//to get rid of infinite loop I'm regenerating "generated" value here every 10 tries
			if (i > 10)
			{
				while (tmpGenerated == generated)
				{
					generated = UnityEngine.Random.Range(1, 10);
				}
				tmpGenerated = generated;
				i = 1;
			}
		}

		//TODO Forbid to print more than one digit for example "9-?=3"


		//For "9-?=3" type of example I rearranging example to put question mark INSIDE
		if (randomToggle == 2)
		{
			correctAnswer.Value = generated + signInt * generated2;

			//if correct answer is 10 I need rearrange example to fix question mark position
			if (correctAnswer.Value == 10)
			{
				if (!isThisSecondAnswer10InRow)
				{
					MoveQuestionMarkInside();
				}
				MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), offset);
				isThisSecondAnswer10InRow = true;
			}
			else
			{
				MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 0);
				MoveQuestionMarkInside();
				isThisSecondAnswer10InRow = false;
			}
			
			mathExamp.text = generated + signStr + "   " + "=" + correctAnswer.Value;

			//real correct answer for example "9-?=3" equals "generated2"
			correctAnswer.Value = generated2;
		}
		else //for other type of examples - "1+5=?" and "6-4=?"
		{
			mathExamp.text = generated + signStr + generated2 + "=";
			correctAnswer.Value = generated + signInt * generated2;
		}
		
		

	}


	void MoveRightSideOfRectTransform(RectTransform rect, float value)
	{
		Vector2 ofs = rect.offsetMax;
		ofs.x = value;
		rect.offsetMax = ofs;
	}


	int ChooseSign()
	{
		if (randomToggle == 2)									//if unknown
		{
			if (toggles10.toggles[0] && toggles10.toggles[1])
			{
				randomToggle = UnityEngine.Random.Range(0, 2);
				if (randomToggle == 1)
				{
					return -1;
				}
				else
				{
					return 1;
				}
			}
			else if (toggles10.toggles[1])
			{
				return -1;
			}
			else
			{
				return 1;
			}
		}
		else if (randomToggle == 1)								//if minus
		{
			return -1;
		}
		else													//if plus
		{
			return 1;
		}
	}

	void GenerateMultiplicationTableExample ()
	{
		// tmpGenerated = generated = 0 after first initialization
		// I'm making next generated value different from previous one (tmpGenerated)
		while (tmpGenerated == generated)
			generated = UnityEngine.Random.Range(1, 11);
		tmpGenerated = generated;

		while (tmpGenerated2 == generated2)
			generated2 = UnityEngine.Random.Range(1, 11);
		tmpGenerated2 = generated2;

		mathExamp.text = generated.ToString() + "×" + generated2.ToString() + "=";
		correctAnswer.Value = generated * generated2;
	}
}
