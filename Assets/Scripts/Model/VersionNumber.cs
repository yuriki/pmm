using UnityEngine;
using UnityEngine.UI;

public class VersionNumber : MonoBehaviour
{
	void Start()
	{
		this.GetComponent<Text>().text = "v" + Application.version;
	}
}
