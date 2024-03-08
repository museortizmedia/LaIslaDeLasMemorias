using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] List<int> _users;
    public List<int> Users {get=>_users; set { _users=value; } }
    public ScriptableUserSprite UserIcons;
    public InteractableArea[] InteractionAreas;

    void MostrarJugadoresUI(){
        Debug.Log("Se actualizó la lista de jugadores");
    }
    public void VerificarJugadoresNuevos(ButtonData buttonData){
        int deviceId = buttonData.DeviceId;
        if(!Users.Contains(deviceId)){
            Users.Add(deviceId);
            MostrarJugadoresUI();
        }
    }
    public void OnInteraction(ButtonData buttonData){
        //
    }
    public void MoveIcon(){
        //
    }
}
