using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    public List<Component> _managers = new List<Component>();
    public ScriptableActivitiesInfo scriptableActivitiesInfo;

    public enum GameState { Splash, Configuration, Inicio, Experiencia, Victory, Fail }
    [SerializeField] GameState _gameState = GameState.Splash;
    public GameState gameState {
        get=>_gameState;
        set{
            _gameState = value;
            ChangeGameState();
        }
    }
    public int _ambienteActual, _actividadActual;


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
        gameState = (GameState)newGameState;
    }
    void ChangeGameState(){
        Debug.Log("GAME: Se cambió el estado de juego a: "+gameState);
        switch(gameState){
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
