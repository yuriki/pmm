using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExampleGenerator : MonoBehaviour 
{
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
		//int len = toggles10.toggles.Length;

		//foreach (var opt in toggles10.toggles)
		//{

		//}

		//choosing subtype of example based on user activated toggles
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


		//if (randomToggle == 0)
		//{
		//	// tmpGenerated = generated = 0 after first initialization
		//	// I'm making next generated value different from previous one (tmpGenerated)
		//	while (tmpGenerated == generated)
		//	{ 
		//		generated = UnityEngine.Random.Range(1, 10);
		//	}
		//	tmpGenerated = generated;

		//	generated2 = UnityEngine.Random.Range(1, 10);
		//	while (/*tmpGenerated2 == generated2 &&*/ (generated2 + generated) > 10)
		//	{
		//		generated2 = UnityEngine.Random.Range(1, 10);
		//	}

		//	mathExamp.text = generated.ToString() + "+" + generated2.ToString() + "=";
		//	correctAnswer.Value = generated + generated2;
		//}
		//else if (randomToggle == 1)
		//{
		//	// tmpGenerated = generated = 0 after first initialization
		//	// I'm making next generated value different from previous one (tmpGenerated)
		//	while (tmpGenerated == generated)
		//	{
		//		generated = UnityEngine.Random.Range(1, 10);
		//	}
		//	tmpGenerated = generated;

		//	generated2 = UnityEngine.Random.Range(1, 10);
		//	while (/*tmpGenerated2 == generated2 &&*/ (generated - generated2) < 0)
		//	{
		//		generated2 = UnityEngine.Random.Range(1, 10);
		//	}

		//	mathExamp.text = generated.ToString() + "-" + generated2.ToString() + "=";
		//	correctAnswer.Value = generated - generated2;
		//}
		//else if (randomToggle == 2)
		//{
		//	// tmpGenerated = generated = 0 after first initialization
		//	// I'm making next generated value different from previous one (tmpGenerated)
		//	while (tmpGenerated == generated)
		//	{
		//		generated = UnityEngine.Random.Range(1, 10);
		//	}
		//	tmpGenerated = generated;

		//	if (toggles10.toggles[0] && toggles10.toggles[1])
		//	{
		//		randomToggle = UnityEngine.Random.Range(0, 2);
		//		if (randomToggle == 1)
		//		{
		//			randomToggle = -1;
		//			sign = "-";
		//		}
		//		else
		//		{
		//			randomToggle = 1;
		//			sign = "+";
		//		}
		//	}
		//	else if (toggles10.toggles[1])
		//	{
		//		randomToggle = -1;
		//		sign = "-";
		//	}
		//	else
		//	{
		//		randomToggle = 1;
		//		sign = "+";
		//	}

		//	generated2 = UnityEngine.Random.Range(1, 10);
		//	while (/*tmpGenerated2 == generated2 &&*/ (generated + randomToggle * generated2) < 0 && (generated + randomToggle * generated2) > 10)
		//	{
		//		generated2 = UnityEngine.Random.Range(1, 10);
		//	}

		//	mathExamp.text = generated.ToString() + sign + "^" + generated2.ToString() + "=";
		//	correctAnswer.Value = generated + randomToggle * generated2;
		//}



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
		// I'm making next generated value different from previous one (tmpGenerated)
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
		//TODO make rearrangement for "10-?=3" type of example
		mathExamp.text = generated + signStr + "^" + generated2 + "=";
		correctAnswer.Value = generated + signInt * generated2;

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
