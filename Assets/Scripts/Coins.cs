using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class Coins : MonoBehaviour
{
	public AudioClip yay;
	public GameObject coinPrefab;

	[Header("States")]
	public StateData correctAnswersNum;

	[Header("DestinationPlaces")]
	[Tooltip("Coordinates where coins must fly to")]
	public Transform coinsDestinationPlace;

	[Header ("Particles")]
	[Tooltip("Particles VFX for coins collecting/disapearing")]
	public GameObject particlesSmallCoins;
	public GameObject pooParticles;

	//I will put all earned coins to this array to animate them 
	[NonSerialized]
	public GameObject[] arrayOfEarnedCoins = new GameObject[10];

	//shaking time
	float syncTime = .4f;

	//current instance of coin
	GameObject theCoin;

	String assetBundleName = "md.md";
	String url = "http://pocketmoneymath.3dyuriki.com/bundles/";
	String platform;

	void Start()
	{
		StartCoroutine(InstantiateObject());
	}

	IEnumerator InstantiateObject()
	{
		#if UNITY_IOS || UNITY_ANDROID
			platform = "Android/";
		#endif

		#if UNITY_EDITOR
			platform = "StandaloneWindows64/";
		#endif

		String uri = url + platform + assetBundleName;
		UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri);



		//with download progress
		request.SendWebRequest();
		while (!request.isDone)
		{
			Debug.Log(request.downloadProgress);
			yield return null;
		}
		Debug.Log(request.downloadProgress);

		//the same as above without download progress
		//yield return request.SendWebRequest();

		AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

		//with download progress
		if (bundle != null)
		{
			AssetBundleRequest newRequest = bundle.LoadAssetAsync<GameObject>("SK_CoinCharacter_md");
			while (!newRequest.isDone)
			{
				Debug.Log("Loading " + newRequest.progress);
				yield return null;
			}
			coinPrefab = (GameObject)newRequest.asset;
		}

		//the same as abouve without download progress
		//coinPrefab = bundle.LoadAsset<GameObject>("SK_CoinCharacter_md");

		yield return null;

	}

	/// <summary>
	/// Cast Coin in random position above top side of screen
	/// </summary>
	/// <returns></returns>
	public IEnumerator DropCoin()
	{
		//creating V3 with random X position to cast coin from
		Vector3 randomV3 = new Vector3((UnityEngine.Random.value - 0.5f) * 4f, 5.4f, 90f);
		//creating random float to use as random scale
		float randomFloat = 0.15f - UnityEngine.Random.value * 0.05f;

		theCoin = GameObject.Instantiate(coinPrefab, randomV3, Quaternion.identity);

		theCoin.transform.position = randomV3;
		theCoin.transform.localScale = new Vector3(randomFloat, randomFloat, 1f);

		PlayRandomWoohooSound();

		SmileWhileFalling();

		yield return null;
	}



	void PlayRandomWoohooSound()
	{
		float randomPitch = 0.8f + UnityEngine.Random.value/2f;
		AudioSource theCoinAudio = theCoin.GetComponent<AudioSource>();
		
		if (randomPitch > 1.1f)
		{
			randomPitch = 0.75f + UnityEngine.Random.value / 2f;
			theCoinAudio.pitch = randomPitch;
			theCoinAudio.PlayOneShot(yay);
		}
		else
		{
			theCoinAudio.pitch = randomPitch;
			theCoin.GetComponent<AudioSource>().Play();
		}
	}

	void SmileWhileFalling ()
	{
		for (int coin = 0; coin < 10; coin++)
		{
			if (arrayOfEarnedCoins[coin])
			{
				arrayOfEarnedCoins[coin].GetComponent<Animator>().SetTrigger("isSmiling");
			}
		}
	}

	///// <summary>
	///// Cast Coin in certain location
	///// </summary>
	///// <param name="location">Certain location for casting Coin</param>
	///// <returns></returns>
	//public IEnumerator DropCoin(Vector3 location)
	//{
	//	//use as random scale (+-10% around 0.5)
	//	float randomFloat = 0.5f - UnityEngine.Random.value * 0.1f;

	//	theCoin = GameObject.Instantiate(coinPrefab, location, Quaternion.identity);

	//	theCoin.transform.position = location;
	//	theCoin.transform.localScale = new Vector3(randomFloat, randomFloat, 1f);

	//	yield return null;
	//}

	public IEnumerator AddCoinToArray(int coinID)
	{
		arrayOfEarnedCoins[coinID] = theCoin;
		yield return null;
	}


	public IEnumerator DestroyCoin(int coinID)
	{
		if (arrayOfEarnedCoins[coinID])
		{
			iTween.ShakePosition(arrayOfEarnedCoins[coinID], iTween.Hash(						//shaking coin
				"x", .2f,
				"y", .05f,
				"time", syncTime));
			iTween.ScaleBy(arrayOfEarnedCoins[coinID], iTween.Hash(								//scaling coin up
				"x", 2f,
				"y", 2f,
				"time", syncTime,
				"EaseType", "easeInCubic"));
			yield return new WaitForSeconds(syncTime);											//waiting for shaking
			pooParticles.transform.position = arrayOfEarnedCoins[coinID].transform.position;	//moving particles to coin position							
			Destroy(arrayOfEarnedCoins[coinID]);
			pooParticles.SetActive(false);														//restarting particles
			pooParticles.SetActive(true);
		}

		yield return null;
	}


	public void ShowScaredFace()
	{
		for (int coin = 0; coin < 10; coin++)
		{
			if (arrayOfEarnedCoins[coin])
			{
				arrayOfEarnedCoins[coin].GetComponent<Animator>().SetTrigger("isScared");
			}
		}
	}


	/// <summary>
	/// запускаю корутин отложенного полёта копеечек
	/// </summary>
	public void CountCoins ()
	{	
		//Starting coroutines for each coin
		for (int i=0; i<correctAnswersNum.Value; i++)
		{
			StartCoroutine(WaitBeforeNextCoin (i));
		}
	}

    /// <summary>	
    /// корутин запускающий полёт копеечек через 0.1 секунду
    /// </summary>
    /// <param name="num">Number of coin</param>
    IEnumerator WaitBeforeNextCoin (int num)
	{
		yield return new WaitForSeconds (num*0.08f);
		StartCoroutine (CoinsAnimationAndPay (num));
	}


    /// корутин анимирующий плавный полёт копеечек
    public IEnumerator CoinsAnimationAndPay (int value)
	{
		GameObject currentCoin = arrayOfEarnedCoins[value]; //here NEW currentCoin variable because animation is playing for plenty of coins in the same time

		//turn off gravity for current coin to fly easily 
		currentCoin.GetComponentInParent<Rigidbody2D>().gravityScale = 0;
		
		for (; Vector3.Distance(currentCoin.transform.position, coinsDestinationPlace.position) > 0.1f; )
		{
			iTween.MoveTo(currentCoin, iTween.Hash("x", coinsDestinationPlace.position.x, "y", coinsDestinationPlace.position.y, "time", .5f, "easetype", "easeInQuad"));
			yield return 0;
		}

		//deleting coins if it is exists
		Destroy(currentCoin); 

		//hiding and showing particles to reload it
		particlesSmallCoins.SetActive (false);
		particlesSmallCoins.SetActive (true);

		//paying money for each coin
		Money money = this.GetComponent<Money>();
		money.PayAndPrint(money.moneyArray.CurrencyAmounts[money.currencyID.Value].RewardsArray[this.GetComponent<ExampleGenerator>().exampleSwitch.Value]);
	}



}

