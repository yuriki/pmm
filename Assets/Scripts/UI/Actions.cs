using System.Collections;
using UnityEngine;

public class Actions : MonoBehaviour
{
	/// <summary>
	/// Coroutine which move object smothly by 1 second.
	/// </summary>
	/// <param name="obj">Object to move</param>
	/// <param name="position">Transform of destination place</param>
	/// <param name="speed">Speed or max distance to move by 1 frame</param>
	/// <returns></returns>
	public IEnumerator Move(GameObject obj, Transform position, float speed)
	{
		float timeT = Time.time;
		for (; Time.time - timeT < 1f;)
		{
			obj.transform.position = Vector3.MoveTowards(obj.transform.position, position.position, speed);
			yield return 0;
		}
	}


	public IEnumerator Boom(GameObject obj, Vector3 scale)
	{
		Vector3 tmpScale = obj.transform.localScale;
		obj.transform.localScale = scale;
		obj.SetActive(true);

		float timeT = Time.time;
		for (; Time.time - timeT < 0.5f;)
		{
			obj.transform.localScale = Vector3.MoveTowards(obj.transform.localScale, tmpScale, 0.1f);
			yield return 0;
		}

		yield return null;
	}
}
