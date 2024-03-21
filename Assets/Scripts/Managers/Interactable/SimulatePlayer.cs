using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimulatePlayer : MonoBehaviour
{
    bool CanChangePlayer = true;
    [SerializeField] int _currentDeviceID = 0;
    public int CurrentDeviceID {
        get => _currentDeviceID;
        set {
            _currentDeviceID = value;
            Debug.LogWarning("SIMULATOR: Ahora interactua como: "+_currentDeviceID);
        }
    }
    Dictionary<KeyCode, UnityAction> _accionesTeclas = new Dictionary<KeyCode, UnityAction>();
    InputManager inputManager;

    private void Start() {
        //Invoke(nameof(CargarAcciones), 1f);
        ConfigAccionesTeclas();

        inputManager = Managers.Instance.GetManager<InputManager>();
    }

    private void Update() {
        if(CanChangePlayer){
            HandleTeclasUsers();
        }
        HanldeTeclasMentor();
    }

    void HandleTeclasUsers(){
        if(Input.GetKeyDown(KeyCode.Keypad0)){
            CurrentDeviceID=0;
        } else if(Input.GetKeyDown(KeyCode.Keypad1)){
            CurrentDeviceID=1;
        } else if(Input.GetKeyDown(KeyCode.Keypad2)){
            CurrentDeviceID=2;
        } else if(Input.GetKeyDown(KeyCode.Keypad3)){
            CurrentDeviceID=3;
        } else if(Input.GetKeyDown(KeyCode.Keypad4)){
            CurrentDeviceID=4;
        } else if(Input.GetKeyDown(KeyCode.Keypad5)){
            CurrentDeviceID=5;
        } else if(Input.GetKeyDown(KeyCode.Keypad6)){
            CurrentDeviceID=6;
        } else if(Input.GetKeyDown(KeyCode.Keypad7)){
            CurrentDeviceID=7;
        } else if(Input.GetKeyDown(KeyCode.Keypad8)){
            CurrentDeviceID=8;
        } else if(Input.GetKeyDown(KeyCode.Keypad9)){
            CurrentDeviceID=9;
        }
    }
    
    void HanldeTeclasMentor(){
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _accionesTeclas[KeyCode.Alpha1]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _accionesTeclas[KeyCode.Alpha2]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _accionesTeclas[KeyCode.Alpha3]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {        _accionesTeclas[KeyCode.Q]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {        _accionesTeclas[KeyCode.W]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {        _accionesTeclas[KeyCode.E]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {        _accionesTeclas[KeyCode.A]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {        _accionesTeclas[KeyCode.S]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {        _accionesTeclas[KeyCode.D]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {        _accionesTeclas[KeyCode.Z]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {        _accionesTeclas[KeyCode.X]?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {        _accionesTeclas[KeyCode.C]?.Invoke();
        }
    }
    void ConfigAccionesTeclas(){
        var teclas = new List<KeyCode>()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.Z,
            KeyCode.X,
            KeyCode.C
        };
        for (int i = 0; i < teclas.Count; i++)
        {
            KeyCode tecla = teclas[i];
            int boton = teclas.IndexOf(tecla);
            UnityAction accion = () =>
            {
                Debug.LogWarning("SIMULADOR: Jugador "+_currentDeviceID+" usó tecla: " + tecla+" que corresponde al indice de eventos "+boton+" y el boton "+(1+ +boton));
                ButtonData newData = new()
                {
                    DeviceId = _currentDeviceID,
                    ButtonId = 1 + +boton
                };
                inputManager.ReciveButtonInteraction(newData);
            };
            _accionesTeclas[tecla] = accion;
        }
    }

}
