using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

public abstract class ExperienceController : MonoBehaviour
{
    //[TextArea] [SerializeField] string instrucciones = "¡Hola! Espero que este mensaje sea suficiente para aprender a usar tu controlador de experiencia.";

    [HideInInspector] public Managers Manager;
    ScriptableIntroMensajes _scriptableIntroMensajes;
    GameObject _canvasIntroPrefab, _canvasIntro;

    //Absurd Game
    public UnityEvent OnStartAbsurdWait, OnEndAnsurdWait;
    
    
    [Header("Inspector de Experience Controller")]
    [Tooltip("Se ejecuta al terminar la Introducción de Intro.")]
    public UnityEvent OnStartExperience;
    [Header("Inspector de Experience Personalizado")]
    public string DevName = "Escribe tu nombre aquí";
    public ScriptableActivitiesData ActivityData;


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
        ActiveInputManager();
        Manager.EstadoGame = Managers.GameState.Configuration;
    }

    public void IniciarCon(int jugadores){
        int i;
        for (i = 1; i <= jugadores; i++)
        {
            Manager.GetManager<InputManager>().ReciveButtonInteraction(new ButtonData{DeviceId = i, ButtonId = 01});
        }
        Debug.Log("Se crearon "+(i-1)+" jugadores para pruebas");
    }

    IEnumerator WaitAbsurdCorrutine(Action CB)
    {
        yield return new WaitForSeconds(ActivityData.AbsurdWait);
        CB?.Invoke();
    }
    public void StartAbsurdCorutine(Action CB){
        StartCoroutine( WaitAbsurdCorrutine(()=>{CB?.Invoke();}));
        Debug.Log("Iniciando tiempo de absurdo");
    }
    public void RestartAbsurdCorutine(Action CB){
        StopAllCoroutines();
        StartCoroutine( WaitAbsurdCorrutine(()=>{CB?.Invoke();}));
        Debug.Log("Reinciando tiempo de absurdo");
    }
    public void StopAbsurdCorutine(){
        StopAllCoroutines();
        Debug.Log("Detenido tiempo de absurdo");
    }

    //HELPERS
    /// <summary>
    /// Combina en Lista1 la lista 2 en posiciones aleatorias segun el percentil aleatorio de ActivityData
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Lista1">ListaBase: Ahí quedará el resultado</param>
    /// <param name="Lista2">Lista que se combinará aleatoriamente en lista 1</param>
    /// <returns>lista con los indices agregados</returns>
    public void CombinarListaAleatorio<T>(List<T> Lista1, List<T> Lista2)
    {
        for (int i = 0; i < Lista2.Count; i++)
        {
            int absurdProbabiily = UnityEngine.Random.Range(0, 100);
            if (absurdProbabiily < ActivityData.AbsurdPercent)
            {
                int random = UnityEngine.Random.Range(0, Lista1.Count);
                Lista1.Insert(random, Lista2[i]);
            }
        }
    }
    /// <summary>
    /// Revuelve aleatoriamente una lista
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">Lista que desea revolver</param>
    public void RevolverLista<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    /// <summary>
    /// Espera los segundos y ejecuta una acción
    /// </summary>
    /// <param name="segundos">Segundos a esperar</param>
    /// <param name="CB">CallBack de la llamada, escribe en un afuncion anónima las acciones que quieres que realice</param>
    public void EsperaYRealiza(float segundos, Action CB){
        StartCoroutine(EsperaYRealizaCorrutina(segundos, CB));
    }
    IEnumerator EsperaYRealizaCorrutina(float segundos, Action CB){
        yield return new WaitForSeconds(segundos);
        CB?.Invoke();
    }
    /// <summary>
    /// Activa las interacciones con el InteractableManager
    /// </summary>
    public void ActiveInputManager()
    {
        Manager.GetManager<InteractableManager>().ChangeInteractionMode(true);
    }
    /// <summary>
    /// Desactiva las interacciones con el InteractableManager
    /// </summary>
    public void DesactiveInputManager()
    {
        Manager.GetManager<InteractableManager>().ChangeInteractionMode(false);
    }
}
