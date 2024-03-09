using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableManager : MonoBehaviour, IManager
{
    [SerializeField] List<int> _users;
    public List<int> Users {get=>_users; set { _users=value; } }
    public ScriptableUserSprite UserIcons;
    [SerializeField] Transform _usersBar;
    public InteractableArea[] InteractionAreas;

    private void Start()
    {
        if(_usersBar==null){_usersBar = transform.GetChild(0).GetChild(0).GetChild(0);}
    }
    void MostrarJugadoresUI(){
        Debug.Log("Se actualizó la lista de jugadores");
        if(!transform.GetChild(0).gameObject.activeSelf){transform.GetChild(0).gameObject.SetActive(true);}
        for (int i = 1; i <= 10; i++)
        {
            _usersBar.GetChild(i-1).gameObject.SetActive(Users.Contains(i));
            _usersBar.GetChild(i-1).gameObject.GetComponent<Image>().sprite = UserIcons.UserSprites[i-1];
        }
    }
    public void VerificarJugadoresNuevos(ButtonData buttonData){
        if(!Users.Contains(buttonData.DeviceId)){
            Users.Add(buttonData.DeviceId);
            MostrarJugadoresUI();
        }
    }
    public void OnInteraction(ButtonData buttonData){
        Debug.Log("Usuario: "+buttonData.DeviceId+" interactuó con: "+buttonData.ButtonId);
        if(InteractionAreas.Length!=0){
            //verificar si es una accion valida para este contexto
            //bool isAcept = false;
            for (int i = 0; i < InteractionAreas.Length; i++)
            {
                InteractableArea interactionArea = InteractionAreas[i];
                if(interactionArea.ButonIdAcepted.Length==0){Debug.LogWarning("Su interactable area no tiene botones compatibles, asigne alguno", interactionArea.transform);return;}

                for (int j = 0; j < interactionArea.ButonIdAcepted.Length; j++)
                {
                    int butonID = interactionArea.ButonIdAcepted[j];
                    if(butonID==buttonData.ButtonId){/*isAcept=true;*/MoveIcon(interactionArea, buttonData.DeviceId);}
                }
            }
            //Debug.Log("La acción seleccionada "+(isAcept==true?"Es valida":"no es valida"));
        }else{
            Debug.LogWarning("No tiene InteractionAreas activas! Asigne sus interaction areas", transform);
        }

    }
    public void MoveIcon(InteractableArea area, int playerId)
    {
        area.UsersVotes.Add(playerId);
        area.VotesCount++;
    }
}
