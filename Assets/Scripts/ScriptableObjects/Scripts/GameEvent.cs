using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "new GameEvent", menuName = "ScriptableObject/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
	private List<GameEventListener> listeners = new List<GameEventListener>();

/// <summary>
/// Ejecuta todas las instrucciones asociadas a este evento
/// </summary>
public void Raise()
{
	for(int i = listeners.Count -1; i >= 0; i--)
	listeners[i].OnEventRaised();
}
/// <summary>
/// Ejecuta una instruccion asociada a este evento
/// </summary>
/// <param name="i">index de la instruccion que desea ejecutar</param>
public void RaiseIndex(int i)
{
	listeners[(listeners.Count-1)-i].OnEventRaised();
}
/// <summary>
/// Actualiza en un index especifico una nueva instrucción
/// </summary>
/// <param name="i"></param>
/// <param name="callAdd"></param>
public void RegisterListenerIndex(int i, UnityAction callAdd)
{
	listeners[(listeners.Count-1)-i].Response.AddListener( callAdd );
}
/// <summary>
/// Añade una nueva instrucción
/// </summary>
/// <param name="listener">GameLsitener nuevo</param>
public void RegisterListener(GameEventListener listener)
{
	listeners.Add(listener);
}
/// <summary>
/// Elimina la instruccion del evento
/// </summary>
/// <param name="listener">GameListener que se elimina del registro</param>
public void UnregisterListener(GameEventListener listener)
{
	listeners.Remove(listener);
}
}