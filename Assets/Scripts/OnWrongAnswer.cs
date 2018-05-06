using UnityEngine;

public class OnWrongAnswer : MonoBehaviour
{
	public NonRangedStateData pooID;
	public GameObject crossHolder;
	public RectTransform cross;
	public StateData levelSwitch;

	Vector2 ofs;
	int minus = 1;

	public void DoOnWrongAnswer()
	{
		if (pooID.Value > 19)
		{
			pooID.Value = 19;
			StartCoroutine(this.GetComponent<Poo>().DestroyPoo(pooID.Value));
			Destroy(this.GetComponent<Poo>().arrayOfPoo[pooID.Value]);
			StartCoroutine(this.GetComponent<Poo>().DropPoo());
			StartCoroutine(this.GetComponent<Poo>().AddPooToArray(pooID.Value));
		}
		else
		{
			StartCoroutine(this.GetComponent<Poo>().DropPoo());
			StartCoroutine(this.GetComponent<Poo>().AddPooToArray(pooID.Value));
		}

		StretchRedCross();
		crossHolder.SetActive(true);
		iTween.PunchScale(crossHolder, iTween.Hash(
			"amount", new Vector3(0.5f, 0.5f, 0f),
			"time", 0.5f));
	}


	void StretchRedCross()
	{
		//choosing right offset to stretch cross
		if (levelSwitch.Value == 2)
		{
			ofs = cross.offsetMin;
			minus = -1;
		}
		else
			ofs = cross.offsetMax;

		//counting number of digits inputed by user
		int userInputLen = this.GetComponent<Check>().usersInputText.text.Length;

		//calculating stretch amount depending on number of digits in answer
		if (userInputLen == 1)
			ofs.x = -131*minus;
		else if (userInputLen == 2)
			ofs.x = -68*minus;
		else if (userInputLen == 3)
			ofs.x = 0;

		//appling stretch to cross
		if (levelSwitch.Value == 2)
			cross.offsetMin = ofs;
		else
			cross.offsetMax = ofs;
	}
}
