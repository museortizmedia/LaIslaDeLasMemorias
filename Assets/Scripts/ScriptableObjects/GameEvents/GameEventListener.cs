using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Scriptable Object GameEvent que quieres asignar")]
    public GameEvent Event;
    [Tooltip("qué deberia hacer cuando se consuma este evento?")]
    public UnityEvent Response;

    private void OnEnable()
    { Event?.RegisterListener(this); }

    private void OnDisable()
    { Event?.UnregisterListener(this); }

    public void OnEventRaised()
    { Response.Invoke(); }

    public void OnEventRais(){
        Event.Raise();
    }

    public void Say(string that){
        Debug.Log(that);
    }

    //Ahora puedes asignar el Scriptable Object GameEvent y llamar a la funcion Raise(), se ejecutará todo lo que esté aquí.
}

