//#define DEBUG_AnimEvents

using UnityEngine;
using UnityEngine.Events;
using System;

public class UI_FollowPath : MonoBehaviour
{
    [Serializable]
    public struct ListaDeRutas
    {
        public string name;
        public RectTransform[] Waypoints;
    }
    [Header("Rutas")]
    public ListaDeRutas[] WaypointsList; // Array de objetos vacíos que definen la ruta
    public int WaypointIndex = 0;

    [Header("Set In Inspector")]
    public RectTransform ImageRect; // RectTransform de la imagen
    public float speed = 10f; // Velocidad a la que se mueve la imagen
    public int repeatCount = 1; // Número de veces que se repite la animación
    public bool loop = false; // Si la imagen debe moverse en bucle o no

    [Header("View In Inspector")]

    [SerializeField] private int currentWaypoint = 0; // Índice del waypoint actual
    [SerializeField] private int currentRepeatCount = 0; // Contador de repeticiones
    [SerializeField] private bool isMoving = false; // Si la imagen se está moviendo o no

    [Header("Events")]

    public UnityEvent OnStart;
    public UnityEvent OnStop;
    public UnityEvent OnChangeWaypoint;
    public UnityEvent OnFinishPath;
    public UnityEvent OnRepeatPath;
    public UnityEvent OnFinishRepeat;
    
    void Start()
    {
        // Obtenemos el RectTransform de la imagen
        if(ImageRect==null)
        {
            ImageRect = GetComponent<RectTransform>();
        }
        
        #if DEBUG_AnimEvents
        OnStart.AddListener(()=>{Debug.Log($"Empezó animación {WaypointIndex}");});
        OnStop.AddListener(()=>{Debug.Log($"Se detuvo la animación {WaypointIndex}");});
        OnChangeWaypoint.AddListener(()=>{Debug.Log($"cambio de dirección de la animación {WaypointIndex} de {currentWaypoint-1} a {currentWaypoint}");});
        OnFinishPath.AddListener(()=>{Debug.Log($"Se terminó los puntos de la animación");});
        OnRepeatPath.AddListener(()=>{Debug.Log($"Se está repitiendo la animación");});
        OnFinishRepeat.AddListener(()=>{Debug.Log($"Finalizó la repetición {WaypointIndex}");});
        #endif

    }

    void Update()
    {
        // Si no hay waypoints o la imagen no se está moviendo, no hacemos nada
        if (WaypointsList.Length == 0 || !isMoving)
            return;

        // Movemos la imagen hacia el waypoint actual
        ImageRect.position = Vector3.MoveTowards(ImageRect.position, WaypointsList[WaypointIndex].Waypoints[currentWaypoint].position, speed * Time.deltaTime);

        // Si hemos llegado al waypoint actual, avanzamos al siguiente
        if (ImageRect.position == WaypointsList[WaypointIndex].Waypoints[currentWaypoint].position)
        {
            currentWaypoint++;
            OnChangeWaypoint?.Invoke();

            // Si hemos llegado al final de la ruta
            if (currentWaypoint >= WaypointsList[WaypointIndex].Waypoints.Length)
            {
                OnFinishPath?.Invoke();
                currentRepeatCount++;

                // Si es un loop, repetir la animación, si no es loop preguntarse si se repitió el número de veces deseado
                if(loop || (!loop && currentRepeatCount < repeatCount))
                {
                    currentWaypoint = 0; // Volvemos al primer waypoint
                    OnRepeatPath?.Invoke();
                    return;
                }

                isMoving = false;
                OnStop?.Invoke();
            }
        }
    }

    [ContextMenu("Script: Iniciar animacion")]
    public void StartMoving()
    {
        isMoving = true;
        currentWaypoint = 0;
        currentRepeatCount = 0;
        OnStart?.Invoke();
    }

    [ContextMenu("Script: Detener animacion")]
    public void StopMoving()
    {
        isMoving = false;
        OnStop?.Invoke();
    }

    public void AddNewPath(string nombre, RectTransform[] path)
    {
        WaypointsList[WaypointsList.Length] =
        new ListaDeRutas()
        {
            name = nombre,
            Waypoints = path

        };
    }
}