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
    [SerializeField] bool _isBrainConect;
    public bool IsBrainConect{
        get=>_isBrainConect;
        set
        {
            _isBrainConect=value;
            if(value)
            {
                OnCerebroDetected.Raise();
                OnBrainConnect?.Invoke();
                CancelInvoke();
                _frecuenciaDeBusqueda=10;
                InvokeRepeating(nameof(SearchForBrain), 1, _frecuenciaDeBusqueda);

            }else{
                OnCerebroUndetect.Raise();
                OnBrainDisconnect?.Invoke();
                CancelInvoke();
                _frecuenciaDeBusqueda=3;
                InvokeRepeating(nameof(SearchForBrain), 1, _frecuenciaDeBusqueda);
            }
        }
    }
    int _frecuenciaDeBusqueda = 1;
    [SerializeField] SerialReader _serialReader;
    public GameEvent OnCerebroDetected, OnCerebroUndetect;


    [Header("Eventos")]
    [SerializeField] UnityEvent OnBrainConnect;
    [SerializeField] UnityEvent  OnBrainDisconnect;

  
    [Header("Eventos Input")]
    public UnityEvent <ButtonData> OnAnyButtonPress;
    [SerializeField] GameEvent[] _gameEvents;

    private void Start() {
        if(_serialReader==null){_serialReader = GetComponentInChildren<SerialReader>();}     
        IsBrainConect=false; 
    }
    void SearchForBrain(){
        _serialReader.SendSerialPortData(SerialReader.CerebroComds.Connect);
    }

    /// <summary>
    /// Recibe información de una desicion de interacción tomada (enviada por el cerebro). Se puede leer usando el SerialReader.
    /// </summary>
    /// <param name="ButData">Estrucutra ButtonData con la info de la interacción</param>
    public void ReciveButtonInteraction(ButtonData ButData){    
        OnAnyButtonPress?.Invoke(ButData);
        _gameEvents[ButData.ButtonId-1]?.Raise(); //-1 porque los botones data son del 1 al 12. Y el arreglo es de 0 a 11
    }
}
