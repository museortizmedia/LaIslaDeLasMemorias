using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExperienceController : MonoBehaviour
{
    //[TextArea] [SerializeField] string instrucciones = "¡Hola! Espero que este mensaje sea suficiente para aprender a usar tu controlador de experiencia.";

    [Header("Inspector de Experience Controller")]
    public Managers Manager;
    [SerializeField] ScriptableIntroMensajes _scriptableIntroMensajes;
    GameObject _canvasIntroPrefab, _canvasIntro;

    public UnityEvent OnStartExperience;
    
    [Header("Inspector de Experience Personalizado")]
    public string DevName = "Escribe tu nombre aquí";

    private void Awake()
    {
        

        //Iniciar Intro
        _canvasIntroPrefab = Resources.Load<GameObject>("Prefabs/CanvasIntro");
        _scriptableIntroMensajes = Resources.Load<ScriptableIntroMensajes>("Scriptables/IntroMensajesScriptable");
        
        
    }

    private void Start() {
        Manager = Managers.Instance;
        Invoke(nameof(StartIntro), 0.5f);
    }

    public void StartIntro(){
        _canvasIntro = Instantiate(_canvasIntroPrefab, transform);
        _canvasIntro.SetActive(false);
        _canvasIntro.GetComponentInChildren<TextBoxController>(true).Dialogs =
        _scriptableIntroMensajes.TodosLosDialogos[Manager._ambienteActual].DialogoActividades[Manager._actividadActual].Dialogo;
        _canvasIntro.GetComponentInChildren<TextBoxController>(true).OnFinish.AddListener( ()=>{OnStartExperience?.Invoke();} );
        
        _canvasIntro.SetActive(true);        
    }

    public void StartExperience(){
        //
    }

    public void EndExperience(){
        //
    }
}
