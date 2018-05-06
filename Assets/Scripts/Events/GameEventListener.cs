using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour 
{
	[Tooltip ("Event that we need to register")]
	public GameEvent Event;

	[Tooltip ("Function to invoke when Event happened")]
	public UnityEvent Function;

	private void OnEnable()
	{
		Event.RegisterListener(this);
	}

	private void OnDisable()
	{
		Event.UnregisterListener(this);
	}

	public void OnEventHappened()
	{
		Function.Invoke();
	}
}
