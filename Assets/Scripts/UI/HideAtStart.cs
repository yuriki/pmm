using UnityEngine;

public class HideAtStart : MonoBehaviour
{
	//TODO make it work even if this object was hidden (right after this obj unveils by button Start fires and hides it again)
	void Start ()
	{
		this.gameObject.SetActive(false);
	}
}
