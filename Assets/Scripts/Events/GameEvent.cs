using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Yuriki's ScriptableObj assets/Game event", fileName="Game event", order = 51)]
public class GameEvent : ScriptableObject
{
	/// <summary>
	/// List of listeners that this event will notify if it is happened
	/// </summary>
	private readonly List <GameEventListener> eventListeners = new List<GameEventListener>();

	public void Do()
	{
		for (int i=eventListeners.Count-1; i>=0; i--)	
			eventListeners[i].OnEventHappened();
	}

	public void RegisterListener(GameEventListener listener)
	{
		if (!eventListeners.Contains(listener))
			eventListeners.Add(listener);
	}

	public void UnregisterListener (GameEventListener listener)
	{
		if (eventListeners.Contains(listener))
			eventListeners.Remove(listener);
	}
}
