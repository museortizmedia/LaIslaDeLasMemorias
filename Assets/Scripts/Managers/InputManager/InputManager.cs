using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct ButtonData
{
    public int DeviceId;
    public int ButtonId;
}
public class InputManager : MonoBehaviour, IManager
{
    [HideInInspector] public SerialReader _serialReader;

    [Header("Eventos Input")]
    public UnityEvent <ButtonData> OnAnyButtonPress;
    [SerializeField] GameEvent[] _gameButtonEvents;

    private void Start()
    {
        if(_serialReader==null)
        {
            _serialReader = GetComponentInChildren<SerialReader>();
        }
    }

    // El Serial Reader se comunica con este método
    public void ReciveButtonInteraction(ButtonData ButData)
    {
        // Se ejecuta evento cuando cualquier boton es presionado 
        OnAnyButtonPress?.Invoke(ButData);

        // Llama al evento del botón en específico
        _gameButtonEvents[ButData.ButtonId-1]?.Raise();

        // Se ejecuta el de zona de interacción. Los botones vienen del serial del 1 al 12, y en el arreglo del 0 al 11
        _gameButtonEvents[ButData.ButtonId-1]?.ButtonRaise(ButData);
    }
}
