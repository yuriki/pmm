using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExampleGenerator : MonoBehaviour 
{
	public float offset;
	public float offsetForTen;
	[Header("States")]
	public StateData exampleSwitch;
	public NonRangedStateData correctAnswer;
	public StateData digitsNumber;

	[Header("Toggles")]
	public ExampleTogglesData toggles5;
	public ExampleTogglesData toggles10;
	public ExampleTogglesData togglesMult;
	public ExampleTogglesData togglesColumn;

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
	public Transform userTopRight;

	int generated; 
	int tmpGenerated;

	int generated2; 
	int tmpGenerated2;

	int randomToggle;
	int signInt;
	string signStr;
	int digitMultTable;

	int minRandomValue;

	// counters to help get rid of infinite while-loop
	int i;
	int j;
	int k;

	bool isFirstTimeWobble = true;


	public void NewExample ()
	{
		//set limit of digits user can input (print on screen)
		digitsNumber.Value = 3;

		inputResultText.text = "";
		questionMark.GetComponent<Text>().text = "?";

		if (exampleSwitch.Value == 0)                   //up to ten example 9-1=?, 9+1=?, 9-?=3
		{
			GenerateCountTo10Example();
		}
		else if (exampleSwitch.Value == 1)				//multiplication table example 3x4=?
		{
			GenerateMultiplicationTableExample();
		}
		else if (exampleSwitch.Value == 2)				//column example
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


	void MoveUserInputMarker(Transform destination)
	{
		inputResultText.transform.position = destination.position;
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


	int ChooseFirstRandomToggle(ExampleTogglesData toglesData)
	{
		i = 1;

		//choosing MIN value for Random function (min value I need to choose only numbers when this is MultTable example)
		if (exampleSwitch.Value == 1)
		{
			minRandomValue = 2;
		}
		else
		{
			minRandomValue = 0;
		}

		randomToggle = UnityEngine.Random.Range(minRandomValue, toglesData.toggles.Length);
		while (!toglesData.toggles[randomToggle])
		{
			if (randomToggle != toglesData.toggles.Length - 1)
			{
				randomToggle++;
			}
			else
			{
				randomToggle = minRandomValue;
			}

			//Foolproof. In case NO toggles were selected, the default value will be chosen (PLUS for all type examples or 2 for MultTable)
			i++;
			if (i > 20) //20 because I want cycle go through twice before exit with default value
			{
				randomToggle = minRandomValue;
				break;
			}

		}
		return randomToggle;
	}


	void GenerateCountTo10Example ()
	{
		//randomly choosing subtype of example based on user activated toggles (in TwoLevelsButton)
		randomToggle = ChooseFirstRandomToggle(toggles10);


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
		
		//initializing counters of while loop cycles
		i = 1;
		j = 1;
		//IMPORTANT I can use expression "==0" here only if I regenerate variable "generated". Oterwise I have infinite loop
		while ((generated + signInt * generated2) <= 0 || (generated + signInt * generated2) > 10 )
		{
			//protection against too often zero result (zero result could be only if we have two the same values 3-3=0, 8-8=0)
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

		//For "9-?=3" type of example I rearranging example to put question mark INSIDE
		if (randomToggle == 2)
		{
			//IMPORTANT changing this value I'm doing imposible for user to input more than 1 digit in answer
			digitsNumber.Value = 1;

			correctAnswer.Value = generated + signInt * generated2;

			//if correct answer is 10 I need rearrange example to fix question mark position
			if (correctAnswer.Value == 10)
			{
				MoveUserInputMarker(userTopLeft);
				MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), offsetForTen);
			}
			else
			{
				MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), offset);
				MoveUserInputMarker(userTopLeft);
			}
			
			mathExamp.text = generated + signStr + "   " + "=" + correctAnswer.Value;

			//real correct answer for example "9-?=3" equals "generated2"
			correctAnswer.Value = generated2;
		}
		else //for other type of examples - "1+5=?" and "6-4=?"
		{
			MoveUserInputMarker(userTopRight);
			MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 0);
			
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
				
				if (UnityEngine.Random.Range(0, 2) == 1)
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
		//randomly choosing subtype of example based on user activated toggles (in TwoLevelsButton)
		randomToggle = ChooseFirstRandomToggle(togglesMult);



		//TODO make generated and generated2 switch places (for mult ONLY and not for divistion): 2x9=? --> 9x2=?


		// tmpGenerated = generated = 0 after first initialization
		// I'm making next generated value different from previous one (tmpGenerated)
		//while (tmpGenerated == generated)
		//	generated = UnityEngine.Random.Range(1, 11);
		//tmpGenerated = generated;

		i = 1;
		j = 1;
		k = 1;
		while (tmpGenerated2 == generated2)
		{
			generated2 = UnityEngine.Random.Range(0, 11);

			//protection against simple examples
			//too often zero value
			if (generated2 == 0)
			{
				if (i < 3)
				{
					generated2 = UnityEngine.Random.Range(0, 11);
				}
				i++;
			}
			//too often one value
			if (generated2 == 1)
			{
				if (j < 3)
				{
					generated2 = UnityEngine.Random.Range(0, 11);
				}
				j++;
			}
			//too often ten value
			if (generated2 == 10)
			{
				if (k < 1)
				{
					generated2 = UnityEngine.Random.Range(0, 11);
				}
				k++;
			}
		}
		tmpGenerated2 = generated2;


		//if division is ON
		if (togglesMult.toggles[0])
		{
			//I'm generating here not two (0 and 1), but 4 values (0, 1, 2, 3). 
			//This means division examples will be generated 3x times more often than multiplication examples
			if (UnityEngine.Random.Range(0, 4) == 0)
			{
				mathExamp.text = randomToggle + "×" + generated2 + "=";
				correctAnswer.Value = randomToggle * generated2;
			}
			else
			{
				mathExamp.text = randomToggle * generated2 + "÷" + randomToggle + "=";
				correctAnswer.Value = generated2;
			}
		}
		else
		{
			mathExamp.text = randomToggle + "×" + generated2 + "=";
			correctAnswer.Value = randomToggle * generated2;
		}


	}
}
