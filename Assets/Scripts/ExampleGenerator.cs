using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExampleGenerator : MonoBehaviour 
{
	[Header("States")]
	public StateData exampleSwitch;
	public NonRangedStateData correctAnswer;

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
		// tmpGenerated = generated = 0 after first initialization
		// I'm making next generated value different from previous one (tmpGenerated)
		while (tmpGenerated == generated)
			generated = UnityEngine.Random.Range(0, 11);

		tmpGenerated = generated;

		mathExamp.text = "10-" + generated.ToString() + "=";
		correctAnswer.Value = 10 - generated;
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
