using System;
using System.Collections;
using UnityEngine;

public class Poo : MonoBehaviour
{
	[Tooltip("Poo prefab")]
	public GameObject poo;
	public GameObject rottenApple;

	[Header("Particles")]
	public GameObject pooParticles;
	public GameObject manyPoosParticles;

	bool isFirstPooExploded = false;

	GameObject thePoo;

	//creating empty array of 40 elements. Because I don't want more than 40 poos on screen
	[NonSerialized]
	public GameObject[] arrayOfPoo = new GameObject[41];

	//shaking time
	float syncTime = .4f;


	public IEnumerator DropPoo()
	{
		//creating random var for poo position and scale
		Vector3 randomPos = new Vector3((UnityEngine.Random.value - 0.5f) * 4f, 5.4f, 90f);
		float randomScale = 0.6f - UnityEngine.Random.value * 0.1f;

#if UNITY_ANDROID
		thePoo = GameObject.Instantiate(rottenApple, randomPos, Quaternion.identity);
#else
		thePoo = GameObject.Instantiate(poo, randomPos, Quaternion.identity);
#endif

		thePoo.transform.position = randomPos;
		thePoo.transform.localScale = new Vector3(randomScale, randomScale, 1f);

		this.GetComponent<Coins>().ShowScaredFace();

		yield return null;
	}


	public IEnumerator AddPooToArray(int pooID)
	{
		while (arrayOfPoo[pooID])
			yield return null;

		arrayOfPoo[pooID] = thePoo;
	}


	public IEnumerator DestroyPoo(int pooID)
	{
		if (arrayOfPoo[pooID])
		{
			iTween.ShakePosition(arrayOfPoo[pooID], iTween.Hash(					//shaking poo
				"x", .2f,
				"y", .05f,
				"time", syncTime));
			iTween.ScaleBy(arrayOfPoo[pooID], iTween.Hash(							//scaling poo up
				"x", 2f,
				"y", 2f,
				"time", syncTime,
				"EaseType", "easeInCubic"));
			yield return new WaitForSeconds(syncTime);								//waiting for shaking
			
			//Destroying Poos only for DEBUGGING purpose. When I need to test Poo's behaviour
			//Destroy(arrayOfPoo[pooID]);

			//Deactivating Poos only for final release. Because deactivating is very fast
			arrayOfPoo[pooID].SetActive(false);

			if (!isFirstPooExploded)
			{
				manyPoosParticles.transform.position = arrayOfPoo[pooID].transform.position;
				manyPoosParticles.SetActive(true);
				isFirstPooExploded = true;
			}
			else
			{
				pooParticles.transform.position = arrayOfPoo[pooID].transform.position; //moving particles to poo position
				pooParticles.SetActive(false);                                          //restarting particles
				pooParticles.SetActive(true);
			}

			
		}
		yield return null;
	}
}
