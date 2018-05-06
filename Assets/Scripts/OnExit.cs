using UnityEngine;

public class OnExit : MonoBehaviour
{
	public BoolData isFirstLoad;
	void OnApplicationQuit ()
	{
		#if UNITY_EDITOR
			isFirstLoad.toggle = true; 
		#endif

		//ReadWriteFTP readwriteftp = new ReadWriteFTP ();
		//string localPath = this.GetComponent<OnLoadLevel>().appPath + "Money.txt";
		//readwriteftp.WriteToFTP ("Money.txt", localPath);
	}
}
