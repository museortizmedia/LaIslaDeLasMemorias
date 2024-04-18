using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableManager : MonoBehaviour, IManager
{
    public bool IsActive = true, IsDebug;
    [SerializeField] List<int> _users;
    public List<int> Users {get=>_users; set { _users=value; } }
    public ScriptableUserSprite UserIcons;
    [SerializeField] Transform _usersBar;
    public List<InteractableArea> InteractionAreas = new List<InteractableArea>();
    Dictionary<int, InteractableArea> _usersVoted = new Dictionary<int, InteractableArea>();
    [SerializeField] GameEvent[] GameButtonsEvents;
    //gameEvents[ButData.ButtonId]?.Raise();

    private void Start()
    {
        if(_usersBar==null){_usersBar = transform.GetChild(0).GetChild(0).GetChild(0);}
        Users.Add(0);
    }
    void MostrarJugadoresUI(){
        if(IsDebug){Debug.Log("Se actualizó la lista de jugadores");}
        if(!transform.GetChild(0).gameObject.activeSelf){transform.GetChild(0).gameObject.SetActive(true);}
        for (int i = 1; i <= 10; i++)
        {
            _usersBar.GetChild(i-1).gameObject.SetActive(Users.Contains(i));
            _usersBar.GetChild(i-1).gameObject.GetComponent<Image>().sprite = UserIcons.UserSprites[i-1];
            _usersBar.GetChild(i-1).gameObject.GetComponent<Image>().color = new Color(255,255,255,0.7f);
        }
    }
    public void VerificarJugadoresNuevos(ButtonData buttonData){
        if(!Users.Contains(buttonData.DeviceId)){
            Users.Add(buttonData.DeviceId);
            MostrarJugadoresUI();
        }
    }
    public void OnInteraction(ButtonData buttonData)
    {
        if (IsActive && InteractionAreas.Count != 0)
        {
            if (IsDebug) { Debug.Log("Usuario: " + buttonData.DeviceId + " interactuó con: " + buttonData.ButtonId); }
            foreach (InteractableArea interactionArea in InteractionAreas)
            {
                if (interactionArea == null)
                {
                    Debug.LogWarning("Su InteractableArea no existe", transform);
                    continue;
                }

                if (interactionArea.ButonIdAcepted.Length == 0)
                {
                    Debug.LogWarning("Su InteractableArea es nula o no tiene botones compatibles, asigne alguno", interactionArea.transform);
                    continue;
                }

                foreach (int buttonID in interactionArea.ButonIdAcepted)
                {
                    if (IsActive && buttonID == buttonData.ButtonId)
                    {
                        // Es decisión del Mentor
                        if (buttonData.DeviceId == 0)
                        {
                            Debug.LogWarning("El Mentor ha hecho una super votación");
                            interactionArea.SuperVoto(buttonData);
                            return;
                        }

                        // No es decisión del Mentor
                        // Registrar voto
                        if (!_usersVoted.ContainsKey(buttonData.DeviceId))
                        {
                            // Registrar voto en el Padre
                            _usersVoted.Add(buttonData.DeviceId, interactionArea);

                            if (IsDebug) { Debug.Log("Se ha agregado el usuario " + buttonData.DeviceId + " al InteractableArea", interactionArea.transform); }
                        }
                        else
                        {
                            // Actualizar voto
                            // Remover voto del anterior InteractableArea
                            if (_usersVoted[buttonData.DeviceId] != null)
                            {
                                _usersVoted[buttonData.DeviceId].UsersVotes.Remove(buttonData.DeviceId);
                                _usersVoted[buttonData.DeviceId].VotesCount = _usersVoted[buttonData.DeviceId].VotesCount; //actualizamos votes para actualizar jugadore UI (revisar si es mejor hacer publico el metodo)
                                //_usersVoted[buttonData.DeviceId].VotesCount--;
                            }
                            // Registrar voto en el Padre
                            _usersVoted[buttonData.DeviceId] = interactionArea;

                            if (IsDebug) { Debug.Log("Se ha actualizado el voto del usuario " + buttonData.DeviceId + " al InteractableArea", interactionArea.transform); }
                        }

                        if (!ReferenceEquals(interactionArea, null))
                        {
                            MoveIcon(interactionArea, buttonData.DeviceId);
                        }
                    }
                }
            }
        }
        else
        {
            if (!IsActive) { Debug.LogWarning("El InputManager está inactivo"); return; }
            Debug.LogWarning("No tiene InteractionAreas activas! Asigne sus interaction areas", transform);
        }
    }

    public void MoveIcon(InteractableArea area, int playerId)
    {
        area.UsersVotes.Add(playerId);
        _usersBar.GetChild(playerId-1).gameObject.GetComponent<Image>().color = new Color(255,255,255,1f);
        if(IsDebug){Debug.Log("Subiendo "+area.VotesCount+" a "+(1+(int)area.VotesCount));}
        area.VotesCount = area.VotesCount<0?1:area.VotesCount++;
    }
    public void ChangeInteractionMode(bool state){
        Managers.Instance.GetManager<InteractableManager>().IsActive = state;
        if(state){
            for (int i = 1; i <= 10; i++)
            {
                _usersBar.GetChild(i-1).gameObject.GetComponent<Image>().color = new Color(255,255,255,0.7f);
            }
        }
    }
    public void AddInteractionArea(InteractableArea interArea){
        InteractionAreas.Add(interArea);
    }
    public void RemoveInteractionArea(InteractableArea interArea){
        InteractionAreas.Remove(interArea);
    }
    public void ClearInteractionAreas(){
        InteractionAreas.Clear();
    }

}
