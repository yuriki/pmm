using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

	public int currentLevelID;
	bool theSame = false;

	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}


	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}


	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex != currentLevelID)
		{
			Destroy(gameObject, 1f);
		}
		else if (scene.buildIndex == currentLevelID && !theSame)
		{
			theSame = !theSame;
		}
		else
		{
			Destroy(gameObject, 1f);
		}
	}


	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	//private void OnLevelWasLoaded(int level)
	//{
	//	if (level != currentLevelID)
	//	{
	//		Destroy(gameObject, 1f);
	//	}
	//	else if (level == currentLevelID && !theSame)
	//	{
	//		theSame = !theSame;
	//	}
	//	else
	//	{
	//		Destroy(gameObject, 1f);
	//	}
	//}



}
