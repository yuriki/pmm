using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
	public void LoadScene (int level) 
	{
		SceneManager.LoadScene (level);
	}
}
