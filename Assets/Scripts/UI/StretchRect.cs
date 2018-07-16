using UnityEngine;

[RequireComponent (typeof(Check))]
public class StretchRect : MonoBehaviour
{
	public StateData exampleSwitch;
	public void StretchRectTransformToMatchUserInput(RectTransform rectangle)
	{
		Vector2 ofs;
		int minus = 1;

		//choosing right offset to stretch cross
		if (IsThisColumnExample())
		{
			ofs = rectangle.offsetMin;
			minus = -1;
		}
		else
		{
			ofs = rectangle.offsetMax;
		}

		//counting number of digits inputed by user
		int userInputLen = this.GetComponent<Check>().usersInputText.text.Length;

		//calculating stretch amount depending on number of digits in answer
		if (userInputLen == 1)
			ofs.x = -131 * minus;
		else if (userInputLen == 2)
			ofs.x = -68 * minus;
		else if (userInputLen == 3)
			ofs.x = 0;

		//appling stretch to cross
		if (IsThisColumnExample())
			rectangle.offsetMin = ofs;
		else
			rectangle.offsetMax = ofs;
	}

	bool IsThisColumnExample()
	{
		return (exampleSwitch.Value == 2);
	}
}

