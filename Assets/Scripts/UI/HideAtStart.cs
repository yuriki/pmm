using UnityEngine;

public class HideAtStart : MonoBehaviour
{
	public GameObject[] secondLvlButtons;

	//TODO make it work even if this object was hidden (right after this obj was unhiden by button, Start fires and hides it again)
	void Start ()
	{
		this.gameObject.SetActive(false);
		secondLvlButtons = this.GetComponent<SecondLvlCircleLaunch>().secondLvlButtons;

		for (int i = 0; i < secondLvlButtons.Length; i++)
		{
			if (secondLvlButtons[i])
			{
				secondLvlButtons[i].SetActive(false);
				secondLvlButtons[i].transform.localScale = new Vector3(0.1f, 0.1f, 1f); 
			}
		}
	}
}
