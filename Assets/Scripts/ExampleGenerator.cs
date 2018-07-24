using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExampleGenerator : MonoBehaviour 
{
	#region Public_Variables
	[Header("States")]
	public StateData exampleSwitch;
	public NonRangedStateData correctAnswer;
	public StateData maxDigitsInUserInput;
	public BoolData isHardExamplesOnly;

	[Header("Toggles")]
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

	[Header("Objects to hide/show")]
	public GameObject flipDigitsToggle;

	[Header("Destination places")]
	public Transform userBottom;
	public Transform userTopMiddle;
	public Transform userTopLeft;
	public Transform userTopRight;
	#endregion

	#region Private_Variables

	int generated1; 
	int tmpGen1;

	int generated2; 
	int tmpGen2;

	// counters to help get rid of infinite while-loop and to make some values more rare
	int i;
	int j;
	int k;

	bool isFirstTimeWobble = true;
	#endregion

	public void NewExample ()
	{
		//set limit of digits user can input (print on screen)
		maxDigitsInUserInput.Value = 3;

		inputResultText.text = "";
		questionMark.GetComponent<Text>().text = "?";

		if (exampleSwitch.Value == 0)								//up to ten example 9-1=?, 9+1=?, 9-?=3
		{
			int maxNum;
			//if toggle "up to 10" is active
			if (toggles10.toggles[3]) 
			{
				maxNum = 10;
			}
			else
			{
				maxNum = 5;
			}
			GenerateCountTo10Example(maxNum);
		}
		else if (exampleSwitch.Value == 1)							//multiplication table example 3x4=?
		{
			GenerateMultTableExample();
		}
		else if (exampleSwitch.Value == 2)							//in column example
		{
			int mult;
			//if toggle "100" is active
			if (togglesColumn.toggles[3])
			{
				mult = 10;
			}
			else
			{
				mult = 1;
			}

			GenerateColumnExample(mult);
			ArrangeColumnExample();
		}

		if (isFirstTimeWobble)
		{
			StartCoroutine(WaitAndWobbleQuestionMark(3f));
			isFirstTimeWobble = false;
		}
	}


	void GenerateCountTo10Example(int maxValue)
	{
		int signInt = 1;
		string signStr = "+";
		// Choose sign based on active toggle (among SecondLvlToggles)
		if (ChooseToggleWithValueRandomly(toggles10) == 1)
		{
			signInt = -1;
			signStr = "-";
		}

		//I'm using array because of .NET 3.5 (still no tuple support)
		int[] tmpArray = GenerateRandomPairWithCondition(maxValue, signInt);
		generated1 = tmpArray[0];
		generated2 = tmpArray[1];

		//For unknown (9-?=3) type of example I rearranging example to put user imput mark INSIDE examle
		if (toggles10.toggles[4])
		{
			if (UnityEngine.Random.Range(0, 10) == 1) //1 of 10 examples will be normal (without unknown)
			{
				PrintNormalExample(signInt, signStr);
			}
			else
			{
				string emptySpace = "   ";
				//IMPORTANT! Set this value to 1 to make imposible for user to input more than one digit in answer
				maxDigitsInUserInput.Value = 1;

				correctAnswer.Value = generated1 + signInt * generated2;

				//if correct answer is 10 (consists of TWO digits) I need rearrange example to fix user input position
				if (correctAnswer.Value == 10)
				{
					MoveUserInputMarker(userTopMiddle);
					MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 128);

					//fix for error 0+?=10 (because I don't have enough space for TOW digit answer)
					if (generated1 == 0 && signInt == 1)
					{
						maxDigitsInUserInput.Value = 2;
						emptySpace = "     ";
						MoveUserInputMarker(userTopLeft);
					}
				}
				else
				{
					MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 65);
					MoveUserInputMarker(userTopMiddle);
				}
				mathExamp.text = generated1 + signStr + emptySpace + "=" + correctAnswer.Value;
				//real correct answer for example "9-?=3" equals "generated2"
				correctAnswer.Value = generated2;
			}
		}
		else //for other type of examples - "1+3=?" and "6-4=?"
		{
			PrintNormalExample(signInt, signStr);
		}
	}


	void PrintNormalExample(int signI, string signS)
	{
		MoveUserInputMarker(userTopRight);
		MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 0);

		mathExamp.text = generated1 + signS + generated2 + "=";
		correctAnswer.Value = generated1 + signI * generated2;
	}

	/// <summary>
	/// Randomly generates two numbers (INT) which meet condition: (gen1 +/- gen2) must be bigger than 0 but smaller than maxInt.
	/// There is built in protection against to often generating zero value.
	/// </summary>
	/// <param name="maxInt">Maximum number that could be generated</param>
	/// <param name="sign">Sign "+" or "-"</param>
	/// <returns></returns>
	int[] GenerateRandomPairWithCondition(int maxInt, int sign)
	{
		int gen1, gen2;

		//Set "correction" value to "1" if you want max generated number = 10 or 5 (to get examples like 10+0=?, 10-3=?).
		//Set it to "0" if you want max generated value = 9 or 4.
		int correction = 1;

		gen1 = UnityEngine.Random.Range(1, maxInt + correction);
		// I'm making next generated value different from previous one
		while (tmpGen1 == gen1)
		{
			gen1 = UnityEngine.Random.Range(0, maxInt + correction);
		}
		tmpGen2 = tmpGen1; //I need to save previous generated value because I'm using it further to regenerate gen1
		tmpGen1 = gen1;

		//I'm controling chance to get 0. Now it is 10%
		gen2 = GenerateIntWithChanceToGetZero(10, maxInt + correction);

		//initializing counter of while-loop cycles
		i = 1;
		j = 1;
		k = 1;
		//IMPORTANT I can use expression "== 0" here only if I regenerate variable "generated". Oterwise I'll have infinite loop in some cases
		//For example, when "gen1 = 9" and previous "gen2 = 1", I can't generate 1 again and I have infinite loop
		while ((gen1 + sign * gen2) <= 0 || (gen1 + sign * gen2) > maxInt)
		{
			k++;

			gen2 = GenerateIntWithChanceToGetZero(3, maxInt + correction);

			//protection against too often zero result (zero result could be only if we have two the same values 0+0=0, 1-1=0 ... 9-9=0)
			//This is not the same as condition gen1==gen2 (!). Because 4+4=8 - is not as simple as 4-4=0
			if ((gen1 + sign * gen2) == 0)
			{
				//I'm controling chance to get 3-3=0 type of example. Now it's 50%
				if (UnityEngine.Random.Range(0, 2) == 1)
				{
					while ((gen1 + sign * gen2) == 0)
					{
						j++;
						gen2 = UnityEngine.Random.Range(0, maxInt + correction);

						//Break of infinite loop with default values
						if (j> 100)
						{
							gen1 = gen2 = 2;
							break;
						}
					}
				}
			}
			else
			{
				gen2 = UnityEngine.Random.Range(0, maxInt + correction);
				i++;
			}

			//to get rid of infinite loop I'm regenerating "gen1" value here every 10 tries
			if (i > 10)
			{
				while (tmpGen2 == gen1)
				{
					gen1 = UnityEngine.Random.Range(0, maxInt + correction);
				}
				tmpGen1 = gen1;
				i = 1;
			}

			//Break of infinite loop with default values
			if (k>100)
			{
				gen1 = gen2 = 2;
				break;
			}
		}

		return new int[] { gen1, gen2 };
	}

	/// <summary>
	/// Generates int value between 0 and "max" with "chance" to get zero
	/// </summary>
	/// <param name="chance">Chance to get zero in percent (1 = 1%). Value must be between 1 to 50</param>
	/// <param name="max">Maximum generated number</param>
	/// <returns></returns>
	int GenerateIntWithChanceToGetZero(float chance, int max)
	{
		int gen;
		int invChance;
		if ( chance >= 50)
		{
			invChance = 2;
		}
		else if (chance <= 0)
		{
			invChance = 100;
		}
		else
		{
			invChance = (int)(100 / chance);
		}
		
		//I'm controling chance to get 0. 
		if (UnityEngine.Random.Range(0, invChance) == 1)
		{
			gen = 0;
		}
		else
		{
			gen = UnityEngine.Random.Range(1, max);
		}
		return gen;
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


	void GenerateMultTableExample()
	{
		//randomly choosing subtype of example based on user activated toggles (among SecondLevelButtons)
		generated1 = ChooseToggleWithValueRandomly(togglesMult);

		generated2 = GenerateSecondMultiplierForMultTable();

		string signStr = "×";
		correctAnswer.Value = generated1 * generated2;
		if (togglesMult.toggles[0]) //if division example (24÷3=?) 
		{
			int chance = 20; //5%
			if (isHardExamplesOnly.toggle)
			{
				chance = 2; //50%
			}

			//some chance to get "×" example, instead of "÷"
			if (UnityEngine.Random.Range(0, chance) != 1)
			{
				signStr = "÷";

				correctAnswer.Value = generated2;
				generated2 = generated1;
				generated1 = correctAnswer.Value * generated1; 
			}
		}
		
		if (togglesMult.toggles[1]) //if example with unknown (6×?=18 or 50÷?=10)
		{
			if (generated1 != 0 && correctAnswer.Value != 0) //protection against 0x?=0 or 0÷?=0
			{
				mathExamp.text = generated1 + signStr + GetEmptySpaceAndArrangeMultUnknownExample() + "=" + correctAnswer.Value;
				correctAnswer.Value = generated2; 
			}
			else
			{
				correctAnswer.Value = 23;
				mathExamp.text = "16+" + GetEmptySpaceAndArrangeMultUnknownExample() + "=23";
				correctAnswer.Value = 7;
			}
		}
		else
		{
			mathExamp.text = generated1 + signStr + generated2 + "=";
		}
	}


	string GetEmptySpaceAndArrangeMultUnknownExample()
	{
		MoveUserInputMarker(userTopMiddle);
		maxDigitsInUserInput.Value = 1;
		string emptySpace = "   ";

		if (generated2 > 9) //TWO digits answer case
		{
			emptySpace = "     ";
			MoveUserInputMarker(userTopLeft);
			maxDigitsInUserInput.Value = 2;
		}

		if (correctAnswer.Value > 9)
		{
			MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 128);
		}
		else
		{
			MoveRightSideOfRectTransform(mathExamp.gameObject.GetComponent<RectTransform>(), 65);
		}

		return emptySpace;
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
					if (i < 2) //chance of getting "0" is 2x times less than any other number
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
	

	void ArrangeColumnExample()
	{
		//flip digits toggle activation
		flipDigitsToggle.GetComponent<Toggle>().interactable = false;
		flipDigitsToggle.SetActive(true);

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


	void GenerateColumnExample(int multiplier)
	{
		int signInt;
		string signStr;

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

		// I'm making next generated value different from previous one (tmpGenerated)
		while (tmpGen1 == generated1)
			generated1 = UnityEngine.Random.Range(1*multiplier*multiplier, 60*multiplier);

		while (tmpGen2 == generated2)
			generated2 = UnityEngine.Random.Range(1, 40*multiplier);

		int i = 0;
		int j = 0;
		int k = 0;
		//TODO make protection against infinite loop when toggle ">100" is active
		while (11*multiplier > generated1 + signInt * generated2 || 99*multiplier < generated1 + signInt * generated2)
		{
			k++;
			//first I'm trying 10x times to regenerate generated2
			if (i != 10)
			{
				i++;
				generated2 = UnityEngine.Random.Range(1, 40 * multiplier);
			}
			else //if I'm not successed with 10x tries, I'm trying to look through all values starting from 1
			{
				if (generated2 == 39 * multiplier)
				{
					//protection againts too often "1" value for generated2 (I'm trying 10 times before accept value "1")
					if (j != 10)
					{
						j++;
						generated2 = UnityEngine.Random.Range(2, 40 * multiplier); //min value is "2" to get rid of "1"s
						// I need to regenerate generated1 if sing is "-" to get rid of infinite while-loop
						if (signInt == -1)
						{
							//min value is 20*multiplier to get rid of small numbers (1-40=? etc)
							generated1 = UnityEngine.Random.Range(20 * multiplier, 60 * multiplier);
						}
					}
					else
					{
						generated2 = 1;
						// I need to regenerate generated1 if sing is "-" to get rid of infinite while-loop
						if (signInt == -1)
						{
							//min value is 20*multiplier to get rid of small numbers (1-40=? etc)
							generated1 = UnityEngine.Random.Range(20*multiplier, 60 * multiplier);
						}
					}
				}
				else
				{
					generated2++;
				}
			}

			while (tmpGen1 == generated1)
				generated1 = UnityEngine.Random.Range(1, 60*multiplier);

			//final STUPID protection. If more than 100 tries I generate simple values
			if (k > 100)
			{
				generated1 = UnityEngine.Random.Range(17, 37);
				generated2 = UnityEngine.Random.Range(8, 16);
				break;
			}
		}

		tmpGen1 = generated1;
		tmpGen2 = generated2;

		mathExamp.text = generated1 + "\n" + signStr + "        \n" + generated2 + "\n" + "-------";
		correctAnswer.Value = generated1 + signInt * generated2;
	}


	int ChooseToggleWithValueRandomly(ExampleTogglesData toglesData)
	{
		int minToggle, maxToggle, randomToggle;
		//choosing MIN and Max value for Random function (because I need to choose only numbers when this is MultTable example)
		if (exampleSwitch.Value == 1) //if MultTable
		{
			minToggle = 2;
			maxToggle = 9;
		}
		else
		{
			minToggle = 0;
			maxToggle = 1;
		}

		randomToggle = UnityEngine.Random.Range(minToggle, maxToggle + 1);
		i = 1;
		//checking if randomly picked toggle is ON
		while (!toglesData.toggles[randomToggle])
		{
			if (randomToggle != maxToggle)
			{
				randomToggle++;
			}
			else
			{
				randomToggle = minToggle;
			}

			//Foolproof. In case NO toggles were selected, the default value will be chosen: "+" (plus) for all type examples or "2" (two) for MultTable
			i++;
			if (i > 10) //10 because I want loop goes through twice before exit with default value
			{
				randomToggle = minToggle;
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
