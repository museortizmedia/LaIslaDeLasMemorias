using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new GameEvent", menuName = "ScriptableObject/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
	private List<GameEventListener> listeners = new List<GameEventListener>();

public void Raise()
{
	for(int i = listeners.Count -1; i >= 0; i--)
	listeners[i].OnEventRaised();
}
public void RaiseIndex(int i)
{
	listeners[(listeners.Count-1)-i].OnEventRaised();
}

public void RegisterListenerIndex(int i, UnityAction callAdd)
{
	listeners[(listeners.Count-1)-i].Response.AddListener( callAdd );
}
public void RegisterListener(GameEventListener listener)
{
	listeners.Add(listener);
}
public void UnregisterListener(GameEventListener listener)
{
	listeners.Remove(listener);
}
}