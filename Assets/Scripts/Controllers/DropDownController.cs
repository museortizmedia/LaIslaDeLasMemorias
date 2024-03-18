using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDownController : MonoBehaviour
{
    public TMP_Dropdown DropAmbiente, DropActividad;
    public TextMeshProUGUI Description;
    public ScriptableActivitiesInfo GameInfo;

    [Space(20)]
    [Header("Slide4")]
    public TextMeshProUGUI NombreActividad, NombrePersonaje, ObjetivoActividad, TiempoActividad;
    public DificultMeter Medidor;
    public Image Personaje;

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
        ScriptableActivitiesInfo.Ambiente.Actividad ConfiguracionActividad = GameInfo.Ambientes[DropAmbiente.value]._activitiesNames[DropActividad.value];
        SetCharacter(ConfiguracionActividad);
    }


    void SetCharacter(ScriptableActivitiesInfo.Ambiente.Actividad ConfiguracionActividad)
    {
        NombreActividad.text = ConfiguracionActividad._name;
        NombrePersonaje.text = ConfiguracionActividad._nombrePersonaje;
        ObjetivoActividad.text = ConfiguracionActividad._desc;
        TiempoActividad.text = (ConfiguracionActividad._tiempoEstimado/60).ToString()+" Minutos";
        Medidor.Dificult = (int)ConfiguracionActividad._dificultad;
        Personaje.sprite = ConfiguracionActividad._personaje;

    }

    public void ChangeToInicio(){
        Managers.Instance.SetGameState((int)Managers.GameState.Inicio);
    }
}
