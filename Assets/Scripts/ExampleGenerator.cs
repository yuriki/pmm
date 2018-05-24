using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExampleGenerator : MonoBehaviour 
{
	#region Public_Variables
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
	#endregion

	#region Private_Variables
	int[] tmpArray;

	int generated1; 
	int tmpGen1;

	int generated2; 
	int tmpGen2;

	int randomToggle;
	int signInt;
	string signStr;

	int minRandomValue;
	int maxRandomValue;

	// counters to help get rid of infinite while-loop and to make some values more rare
	int i;
	int j;
	int k;

	bool isFirstTimeWobble = true;
	#endregion

	public void NewExample ()
	{
		//set limit of digits user can input (print on screen)
		digitsNumber.Value = 3;

		inputResultText.text = "";
		questionMark.GetComponent<Text>().text = "?";

		if (exampleSwitch.Value == 0)								//up to ten example 9-1=?, 9+1=?, 9-?=3
		{
			//if toggle "up to 10" is active
			if (toggles10.toggles[3]) 
			{
				maxRandomValue = 10;
			}
			else
			{
				maxRandomValue = 5;
			}
			//if toggle "?" (example with unknown 9-?=3) is active
			if (toggles10.toggles[4]) 
			{
				GenerateCountTo10Example(maxRandomValue, true);
			}
			else
			{
				GenerateCountTo10Example(maxRandomValue, false);
			}
		}
		else if (exampleSwitch.Value == 1)							//multiplication table example 3x4=?
		{
			if (togglesMult.toggles[0]) //if toggle "÷" is active
			{
				GenerateMultTableExample(true); 
			}
			else
			{
				GenerateMultTableExample(false);
			}
		}
		else if (exampleSwitch.Value == 2)							//in column example
		{
			//if toggle "100" is active
			if (togglesColumn.toggles[3])
			{
				maxRandomValue = 1000;
				minRandomValue = 101;
			}
			else
			{
				maxRandomValue = 100;
				minRandomValue = 12;
			}

			GenerateColumnExample();
			ArrangeColumnExample();
		}

		if (isFirstTimeWobble)
		{
			StartCoroutine(WaitAndWobbleQuestionMark(3f));
			isFirstTimeWobble = false;
		}
	}


	void GenerateCountTo10Example(int maxValue, bool isHardExample)
	{
		// Choosing sign based on active toggle (among SecondLvlToggles)
		if (ChooseToggleWithValueRandomly(toggles10) == 1)
		{
			signInt = -1;
			signStr = "-";
		}
		else
		{
			signInt = 1;
			signStr = "+";
		}

		tmpArray = GenerateRandomPair(maxValue);
		generated1 = tmpArray[0];
		generated2 = tmpArray[1];

		//For "9-?=3" type of example I rearranging example to put user imput mark INSIDE examle
		if (isHardExample)
		{
			if (UnityEngine.Random.Range(0, 5) == 0) //1 of 5 examples will be normal (without unknown)
			{
				PrintNormalExample();
			}
			else
			{
				//IMPORTANT! changing this value I'm doing imposible for user to input more than one digit in answer
				digitsNumber.Value = 1;

				correctAnswer.Value = generated1 + signInt * generated2;

				//if correct answer is 10 I need rearrange example to fix user input position
				if (correctAnswer.Value == 10)
				{
					MoveUserInputMarker(userTopLeft);
					MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 128);
				}
				else
				{
					MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 65);
					MoveUserInputMarker(userTopLeft);
				}
				mathExamp.text = generated1 + signStr + "   " + "=" + correctAnswer.Value;
				//real correct answer for example "9-?=3" equals "generated2"
				correctAnswer.Value = generated2;
			}
		}
		else //for other type of examples - "1+3=?" and "6-4=?"
		{
			PrintNormalExample();
		}
	}


	void PrintNormalExample()
	{
		MoveUserInputMarker(userTopRight);
		MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 0);

		mathExamp.text = generated1 + signStr + generated2 + "=";
		correctAnswer.Value = generated1 + signInt * generated2;
	}


	int[] GenerateRandomPair(int maxInt)
	{
		int gen1, gen2;
		int correction = 1; //1 - if I want to max generated number = 10 (or 5): 10+0=?, 10-3=?. Else (max generated value = 9) correction = 0

		gen1 = UnityEngine.Random.Range(0, maxInt + correction);
		i = 1;
		// I'm making next generated value different from previous one (different from tmpGenerated)
		while (tmpGen1 == gen1)
		{
			gen1 = UnityEngine.Random.Range(0, maxInt + correction);

			//protection against too often zero value
			if (gen1 == 0)
			{
				if (i != 2) //two tries before take zero value
				{
					gen1 = UnityEngine.Random.Range(0, maxInt + correction);
					i++;
				}
			}
		}
		tmpGen1 = gen1;

		gen2 = UnityEngine.Random.Range(0, maxInt + correction);
		//protection against too often zero value
		if (gen2 == 0)
		{
			gen2 = UnityEngine.Random.Range(0, maxInt + correction);
		}

		//initializing counters of while-loop cycles
		i = 1;
		j = 1;
		//IMPORTANT I can use expression "== 0" here only if I regenerate variable "generated". Oterwise I'll have infinite loop in some cases
		//For example, when "gen1 = 9" and previous "gen2 = 1", I can't generate 1 again and I have infinite loop
		while ((gen1 + signInt * gen2) <= 0 || (gen1 + signInt * gen2) > maxInt)
		{
			//protection against too often zero result (zero result could be only if we have two the same values 0+0=0, 3-3=0, 8-8=0)
			if ((gen1 + signInt * gen2) == 0)
			{
				if (j > 3) //3 tries
				{
					j = 1;
					break; //I breake while loop with gen1=gen2
				}
				j++;
			}

			gen2 = UnityEngine.Random.Range(0, maxInt + correction);
			i++;
			//to get rid of infinite loop I'm regenerating "gen1" value here every 10 tries
			if (i > 10)
			{
				while (tmpGen1 == gen1)
				{
					gen1 = UnityEngine.Random.Range(0, maxInt + correction);
				}
				tmpGen1 = gen1;
				i = 1;
			}
		}

		return new int[] { gen1, gen2 };
	}


	void MoveRightSideOfRectTransform(RectTransform rect, float value)
	{
		Vector2 ofs = rect.offsetMax;
		ofs.x = value;
		rect.offsetMax = ofs;
	}


	void MoveUserInputMarker(Transform destination)
	{
		inputResultText.transform.position = destination.position;
	}


	void GenerateMultTableExample(bool isHardExample)
	{
		//randomly choosing subtype of example based on user activated toggles (among SecondLevelButtons)
		generated1 = ChooseToggleWithValueRandomly(togglesMult);

		//TODO make generated and generated2 switch places (for mult ONLY and not for divistion): 2x9=? --> 9x2=?

		generated2 = GenerateSecondMultiplierForMultTable();

		//if division "÷" is ON
		if (isHardExample)
		{
			//I'm generating here not two (0 and 1), but 5 values (0, 1, 2, 3, 4). 
			//This means division examples will be generated 4x times more often than multiplication examples
			if (UnityEngine.Random.Range(0, 5) == 0) //1 of 5 examples will be X×Y=? (all other - X÷Y=?)
			{
				PrintMultExample();
			}
			else
			{
				if (UnityEngine.Random.Range(0, 10) == 0) //1 of 10 examples will be X÷1=X
				{
					mathExamp.text = generated1 * generated2 + "÷1=";
					correctAnswer.Value = generated2 * generated2;
				}
				else
				{
					//Because generated1 could be (2-9 inclusive) there is no chance of division by zero example (X÷0=?)
					mathExamp.text = generated1 * generated2 + "÷" + generated1 + "=";
					correctAnswer.Value = generated2;
				}
			}
		}
		else
		{
			PrintMultExample();
		}
	}

	/// <summary>
	/// Generate and return random value (0-10 inclusive) but with control of rare values.
	/// Rare values (0, 1, 10) have smaller chance to be generated.
	/// </summary>
	/// <returns></returns>
	int GenerateSecondMultiplierForMultTable()
	{
		bool checkFailed = true;
		int gen = UnityEngine.Random.Range(0, 11);

		while (checkFailed)
		{
			if (gen == tmpGen2) //new generated value must be different from previous one
			{
				gen = UnityEngine.Random.Range(0, 11);
			}
			else
			{
				//protection against simple examples
				if (gen == 0)//too often "0" value
				{
					if (i < 4) //chance of getting "0" is 4x times less than any other number
					{
						gen = UnityEngine.Random.Range(0, 11);
					}
					else
					{
						i = 0;
						checkFailed = false;
					}
					i++;
				}
				else if (gen == 1)//too often "1" value
				{
					if (j < 2)
					{
						gen = UnityEngine.Random.Range(0, 11);
					}
					else
					{
						j = 0;
						checkFailed = false;
					}
					j++;
				}
				else if (gen == 10)//too often "10" value
				{
					if (k < 2)
					{
						gen = UnityEngine.Random.Range(0, 11);
					}
					else
					{
						k = 0;
						checkFailed = false;
					}
					k++;
				}
				else
				{
					checkFailed = false;
				}
			}
		}
		tmpGen2 = gen;
		return gen;
	}


	void PrintMultExample()
	{
		mathExamp.text = generated1 + "×" + generated2 + "=";
		correctAnswer.Value = generated1 * generated2;
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


	void GenerateColumnExample()
	{
		if (ChooseToggleWithValueRandomly(togglesColumn) == 1)
		{
			signInt = -1;
			signStr = "-";
		}
		else
		{
			signInt = 1;
			signStr = "+";
		}

		generated1 = UnityEngine.Random.Range(1, 20);
		generated2 = UnityEngine.Random.Range(1, 19);
		//while (minRandomValue > generated1 + generated2 || maxRandomValue < generated1 + generated2)
		//{
		//	if (generated2 == 550)
		//	{
		//		generated2 = 1;
		//	}
		//	else
		//	{
		//		generated2++;
		//	}

		//	while (tmpGen1 == generated1)
		//		generated1 = UnityEngine.Random.Range(1, 50);

		//	while (tmpGen2 == generated2)
		//		generated2 = UnityEngine.Random.Range(1, 550);
		//}
		tmpGen1 = generated1;
		tmpGen2 = generated2;

		// I'm making next generated value different from previous one (tmpGenerated)
		//while (tmpGen1 == generated1)
		//	generated1 = UnityEngine.Random.Range(10, 801);
		//tmpGen1 = generated1;

		//while (tmpGen2 == generated2)
		//	generated2 = UnityEngine.Random.Range(1, 200);
		//tmpGen2 = generated2;

		mathExamp.text = generated1.ToString() + "\n" + "+        \n" + generated2.ToString() + "\n" + "-------";
		correctAnswer.Value = generated1 + generated2;
	}


	int ChooseToggleWithValueRandomly(ExampleTogglesData toglesData)
	{
		//choosing MIN and Max value for Random function (because I need to choose only numbers when this is MultTable example)
		if (exampleSwitch.Value == 1) //if MultTable
		{
			minRandomValue = 2;
			maxRandomValue = 9;
		}
		else
		{
			minRandomValue = 0;
			maxRandomValue = 1;
		}

		randomToggle = UnityEngine.Random.Range(minRandomValue, maxRandomValue + 1);
		i = 1;
		//checking if randomly picked toggle is ON
		while (!toglesData.toggles[randomToggle])
		{
			if (randomToggle != maxRandomValue)
			{
				randomToggle++;
			}
			else
			{
				randomToggle = minRandomValue;
			}

			//Foolproof. In case NO toggles were selected, the default value will be chosen: "+" (plus) for all type examples or "2" (two) for MultTable
			i++;
			if (i > 10) //10 because I want loop goes through twice before exit with default value
			{
				randomToggle = minRandomValue;
				break;
			}

		}
		return randomToggle;
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
}
