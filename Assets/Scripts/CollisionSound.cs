using UnityEngine;
using System.Collections;

public class CollisionSound : MonoBehaviour
{
	public AudioClip collisionClip;

	AudioSource source;
	float collisionVolume;


	void Start ()
	{
		source = GetComponent <AudioSource>();
	}
	
	void OnCollisionEnter2D (Collision2D coll)
	{
		collisionVolume = coll.relativeVelocity.magnitude*0.075f;

		if (collisionVolume > 1)
		{
			source.pitch = 1f;
		}
		else
		{
			source.pitch = 1f + collisionVolume;
		}
		source.PlayOneShot (collisionClip, collisionVolume);
	}
}
