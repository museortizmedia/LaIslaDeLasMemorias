using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownController : MonoBehaviour
{
    public TMP_Dropdown DropAmbiente, DropActividad;
    public TextMeshProUGUI Description;
    public ScriptableActivitiesInfo GameInfo;

    private void Start() {
        GameInfo = Resources.Load<ScriptableActivitiesInfo>("Scriptables/ActivitiesInfoScriptable");
    }
    
    public void SetAmbientsDrop(){
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var ambiente in GameInfo.Ambientes)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(ambiente._name));
        }
        DropAmbiente.options = dropdownOptions;
    }
    public void SetActivitiesDrop(){
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (var actividad in GameInfo.Ambientes[DropAmbiente.value]._activitiesNames)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(actividad._name));
        }
        DropActividad.options = dropdownOptions;
    }

    public void SetActivityDescription(int activityindex){
        Description.text = GameInfo.Ambientes[DropAmbiente.value]._activitiesNames[activityindex]._desc;
    }
    public void SetManagerconfig(){
        Managers.Instance.AmbienteActual = DropAmbiente.value;
        Managers.Instance.ActividadActual = DropActividad.value;
    }

    public void ChangeToInicio(){
        Managers.Instance.SetGameState((int)Managers.GameState.Inicio);
    }
}
