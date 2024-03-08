using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct ButtonData
{
    public int DeviceId;
    public int ButtonId;
}
public class InputManager : MonoBehaviour
{
    [SerializeField] bool _isBrainConect;
    public bool IsBrainConect{get=>_isBrainConect; set=>_isBrainConect=value; }

    [SerializeField] SerialReader _serialReader;
    
    [Header("Eventos de Entrada")]
    [SerializeField] UnityEvent OnBrainConnect, OnBrainDisconnect;

    [Header("Eventos Input")]
    Dictionary<KeyCode, UnityAction> _accionesTeclas = new Dictionary<KeyCode, UnityAction>();
    public UnityEvent<ButtonData>
    OnButtonAPress,
    OnButtonBPress,
    OnButtonCPress,
    OnButton1Press,
    OnButton2Press,
    OnButton3Press,
    OnButton4Press,
    OnButton5Press,
    OnButton6Press,
    OnButton7Press,
    OnButton8Press,
    OnButton9Press,
    //OnButton10Press,
    //OnButton11Press,
    //OnButton12Press,
    OnCanalButton1Press,
    OnCanalButton2Press,
    OnCanalButton3Press,
    OnAnyButtonPress;

    

    private void Start() {
        if(_serialReader==null){_serialReader = GetComponentInChildren<SerialReader>();}

        _accionesTeclas[KeyCode.Alpha1] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=1});
            OnButtonAPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=1});
        };
        _accionesTeclas[KeyCode.Alpha2] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=2});
            OnButtonBPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=2});
        };
        _accionesTeclas[KeyCode.Alpha3] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=3});
            OnButtonCPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=3});
        };

        _accionesTeclas[KeyCode.Q] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=4});
            OnCanalButton1Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=4});
            OnButton1Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=4});
        };
        _accionesTeclas[KeyCode.W] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=5});
            OnCanalButton2Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=5});
            OnButton2Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=5});
        };
        _accionesTeclas[KeyCode.E] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=6});
            OnCanalButton3Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=6});
            OnButton3Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=6});
        };
        _accionesTeclas[KeyCode.A] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=7});
            OnCanalButton1Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=7});
            OnButton4Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=7});
        };
        _accionesTeclas[KeyCode.S] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=8});
            OnCanalButton2Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=8});
            OnButton5Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=8});
        };
        _accionesTeclas[KeyCode.D] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=9});
            OnCanalButton3Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=9});
            OnButton6Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=9});
        };
        _accionesTeclas[KeyCode.Z] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=10});
            OnCanalButton1Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=10});
            OnButton7Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=10});
        };
        _accionesTeclas[KeyCode.X] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=11});
            OnCanalButton2Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=11});
            OnButton8Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=11});
        };
        _accionesTeclas[KeyCode.C] = ()=>{
            OnAnyButtonPress?.Invoke(new ButtonData{DeviceId=0, ButtonId=12});
            OnCanalButton3Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=12});
            OnButton9Press?.Invoke(new ButtonData{DeviceId=0, ButtonId=12});
        };
    }

    public void ReciveButtonInteraction(ButtonData ButData){
    
        Dictionary<int, UnityAction<ButtonData>> accionesPorButtonId = new Dictionary<int, UnityAction<ButtonData>>
        {
            { 1, (data) => { OnButtonAPress?.Invoke(data); } },
            { 2, (data) => { OnButtonBPress?.Invoke(data); } },
            { 3, (data) => { OnButtonCPress?.Invoke(data); } },
            { 4, (data) => { OnButton1Press?.Invoke(data); OnCanalButton1Press?.Invoke(data); } },
            { 5, (data) => { OnButton2Press?.Invoke(data); OnCanalButton2Press?.Invoke(data); } },
            { 6, (data) => { OnButton3Press?.Invoke(data); OnCanalButton3Press?.Invoke(data); } },
            { 7, (data) => { OnButton4Press?.Invoke(data); OnCanalButton1Press?.Invoke(data); } },
            { 8, (data) => { OnButton5Press?.Invoke(data); OnCanalButton2Press?.Invoke(data); } },
            { 9, (data) => { OnButton6Press?.Invoke(data); OnCanalButton3Press?.Invoke(data); } },
            { 10, (data) => { OnButton7Press?.Invoke(data); OnCanalButton1Press?.Invoke(data); } },
            { 11, (data) => { OnButton8Press?.Invoke(data); OnCanalButton2Press?.Invoke(data); } },
            { 12, (data) => { OnButton9Press?.Invoke(data); OnCanalButton3Press?.Invoke(data); } }
        };

        // Invoca la acción correspondiente si existe en el diccionario
        if (accionesPorButtonId.ContainsKey(ButData.ButtonId))
        {
            OnAnyButtonPress?.Invoke(ButData);
            accionesPorButtonId[ButData.ButtonId]?.Invoke(ButData);
        }
        else
        {
            Debug.LogWarning("ButtonId fuera de rango.");
        }
        
    }

}
