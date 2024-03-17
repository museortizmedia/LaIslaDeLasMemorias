using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ExperienceController : MonoBehaviour
{
    //[TextArea] [SerializeField] string instrucciones = "¡Hola! Espero que este mensaje sea suficiente para aprender a usar tu controlador de experiencia.";

    [HideInInspector] public Managers Manager;
    ScriptableIntroMensajes _scriptableIntroMensajes;
    GameObject _canvasIntroPrefab, _canvasIntro;
    
    
    [Header("Inspector de Experience Controller")]
    [Tooltip("Se ejecuta al terminar la Introducción de Intro.")]
    public UnityEvent OnStartExperience;
    [Header("Inspector de Experience Personalizado")]
    public string DevName = "Escribe tu nombre aquí";


    protected virtual void Awake()
    {
        //Iniciar Intro
        _canvasIntroPrefab = Resources.Load<GameObject>("Prefabs/CanvasIntro");
        _scriptableIntroMensajes = Resources.Load<ScriptableIntroMensajes>("Scriptables/IntroMensajesScriptable");
    }
    public virtual void Start(){
        Manager = Managers.Instance;
        Invoke(nameof(StartIntro), 0.5f);
        _canvasIntro = Instantiate(_canvasIntroPrefab, transform);
        _canvasIntro.SetActive(false);
        Debug.Log("Se incializó ExperienceController");
    }

    /// <summary>
    /// Inicia la introducción del Centro usando TextBox
    /// </summary>
    void StartIntro(){

        _canvasIntro.GetComponentInChildren<TextBoxController>(true).Dialogs =
        _scriptableIntroMensajes.TodosLosDialogos[Manager.AmbienteActual].DialogoActividades[Manager.ActividadActual].Dialogo;
        _canvasIntro.GetComponentInChildren<TextBoxController>(true).OnFinish.AddListener( ()=>{ Destroy(_canvasIntro); OnStartExperience?.Invoke();} );
        _canvasIntro.SetActive(true);        
    }

    public virtual void EndExperience(){
        Manager.EstadoGame = Managers.GameState.Configuration;
    }
}
