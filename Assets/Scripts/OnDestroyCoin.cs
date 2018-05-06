using UnityEngine;

public class OnDestroyCoin : MonoBehaviour 
{
	// Deleting coin and generating new math example
	public void DestroyCoinLauncher(StateData coinID)
	{
		StartCoroutine(this.GetComponent<Coins>().DestroyCoin(coinID.Value));

		this.GetComponent<ExampleGenerator>().NewExample();
	}
}
