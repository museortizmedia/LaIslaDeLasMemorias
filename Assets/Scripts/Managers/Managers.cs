using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    public List<Component> _managers = new List<Component>();

    public enum GameState { Splash, Configuration, Inicio, Experiencia, Victory, Fail }
    [SerializeField] GameState _gameState = GameState.Splash;
    public GameState gameState {
        get=>_gameState;
        set{
            Debug.Log(value);
            _gameState = value;
            ChangeGameState();
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Component[] children = GetComponentsInChildren<Component>();
        foreach (Component child in children)
        {
            if (child is IManager manager)
            {
                _managers.Add((Component)manager);
            }
        }
    }
    public void SetGameState(int newGameState){
        gameState = (GameState)newGameState;
    }
    void ChangeGameState(){
        Debug.Log("Cambio de estado de juego a: "+gameState);
        switch(gameState){
            case GameState.Configuration:
            SceneManager.LoadScene(1);
            break;
            default:
            break;
        }
    }
}
