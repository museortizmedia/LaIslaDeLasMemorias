using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    public List<Component> _managers = new List<Component>();
    public ScriptableActivitiesInfo scriptableActivitiesInfo;

    public enum GameState { Splash, Configuration, Inicio }
    [SerializeField] GameState _gameState = GameState.Splash;
    public GameState EstadoGame {
        get=>_gameState;
        set{
            _gameState = value;
            ChangeGameState();
        }
    }
    [SerializeField] int _ambienteActual, _actividadActual;
    public int AmbienteActual {get=>_ambienteActual; set=>_ambienteActual=value;}
    public int ActividadActual {get=>_actividadActual; set=>_actividadActual=value;}


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        Component[] children = GetComponentsInChildren<Component>();
        foreach (Component child in children)
        {
            if (child is IManager manager)
            {
                _managers.Add((Component)manager);
            }
        }
    }

    private void Start()
    {
        scriptableActivitiesInfo = Resources.Load<ScriptableActivitiesInfo>("Scriptables/ActivitiesInfoScriptable");
    }
    public T GetManager<T>() where T : Component
    {
        foreach (var manager in _managers)
        {
            if (manager is T)
            {
                return (T)manager;
            }
        }
        return null;
    }
    public void SetGameState(int newGameState){
        EstadoGame = (GameState)newGameState;
    }
    void ChangeGameState(){
        Debug.Log("GAME: Se cambió el estado de juego a: "+EstadoGame);
        switch(EstadoGame){
            case GameState.Configuration:
            Debug.Log("GAME: Se carga la escena de configuración, se activan los managers");
            SceneManager.LoadScene("Configuracion");
            _ambienteActual = 0; _actividadActual=0;
            break;
            case GameState.Inicio:
            SceneManager.LoadScene(scriptableActivitiesInfo.Ambientes[_ambienteActual]._activitiesNames[_actividadActual]._scene.name);
            break;
            default:
            break;
        }
    }
    public void DebugLog(string text){
        Debug.Log(text);
    }
}
