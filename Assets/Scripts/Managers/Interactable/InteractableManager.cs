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
    public List<InteractableArea> InteractionAreas = new List<InteractableArea>();

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
        if(InteractionAreas.Count!=0){
            //verificar si es una accion valida para este contexto
            //bool isAcept = false;
            foreach (InteractableArea interactionArea in InteractionAreas)
            {
                if (interactionArea == null)
                {
                    Debug.LogWarning("Su interactable area no existe", transform);
                    continue;
                }

                if (interactionArea.ButonIdAcepted.Length == 0)
                {
                    Debug.LogWarning("Su interactable area es nula o no tiene botones compatibles, asigne alguno", interactionArea.transform);
                    continue;
                }

                foreach (int buttonID in interactionArea.ButonIdAcepted)
                {
                    if (buttonID == buttonData.ButtonId)
                    {
                        MoveIcon(interactionArea, buttonData.DeviceId);
                    }
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
    public void AddInteractionArea(InteractableArea interArea){
        InteractionAreas.Add(interArea);
    }
    public void AddInteractionRemove(InteractableArea interArea){
        InteractionAreas.Remove(interArea);
    }
    public void ClearInteractionAreas(){
        InteractionAreas.Clear();
    }
}
