using UnityEngine;
//using System.Collections;

public class OnDestroyPoo : MonoBehaviour
{
	//I'm using DestroyPooLauncher instead of DestroyPoo because I can't launch coroutine from event
	public void DestroyPooLauncher(NonRangedStateData pooID)
	{
		//Deleting poo from global array of poos
		StartCoroutine(this.GetComponent<Poo>().DestroyPoo(pooID.Value));

		this.GetComponent<ExampleGenerator>().NewExample();
	}
}
