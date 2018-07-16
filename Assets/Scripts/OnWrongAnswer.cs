using UnityEngine;

[RequireComponent(typeof(StretchRect))]
public class OnWrongAnswer : MonoBehaviour
{
	public NonRangedStateData pooID;
	public GameObject crossHolder;
	public RectTransform cross;

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

		this.GetComponent<StretchRect>().StretchRectTransformToMatchUserInput(cross);

		crossHolder.SetActive(true);
		iTween.PunchScale(crossHolder, iTween.Hash(
			"amount", new Vector3(0.5f, 0.5f, 0f),
			"time", 0.5f));
	}
}
