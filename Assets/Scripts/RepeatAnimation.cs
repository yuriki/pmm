using UnityEngine;
using System.Collections;

public class RepeatAnimation : MonoBehaviour
{
	Animator animatorComponent;
	
	void Start ()
	{
		animatorComponent = this.GetComponent<Animator>();
		StartCoroutine (StartAnimationAtRandomTime_InfiniteLoop());
	}


	IEnumerator StartAnimationAtRandomTime_InfiniteLoop () 
	{
		while (true)
		{
			animatorComponent.SetTrigger("StartAnimation");
			yield return new WaitForSeconds(Random.Range(7, 15));
		}
	}

	IEnumerator SwitchClosedEyes ()
	{
		animatorComponent.SetTrigger("Blink");
		yield return new WaitForSeconds(1f);
		animatorComponent.SetTrigger("OpenEye");
	}
}
