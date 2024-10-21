using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActionsController : MonoBehaviour
{
    public UnityEvent[] devicesEnter = new UnityEvent[10];
    public UnityEvent[] devicesExit = new UnityEvent[10];

    // Cola para almacenar las órdenes de invocar eventos en el hilo principal
    private Queue<System.Action> mainThreadQueue = new Queue<System.Action>();

    private void Awake()
    {
        // Asegurarse de que los eventos estén inicializados
        for (int i = 0; i < devicesEnter.Length; i++)
        {
            if (devicesEnter[i] == null)
            {
                devicesEnter[i] = new UnityEvent();
            }
        }

        for (int i = 0; i < devicesExit.Length; i++)
        {
            if (devicesExit[i] == null)
            {
                devicesExit[i] = new UnityEvent();
            }
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(AddEvents), 1f);
    }

    private void OnDisable()
    {
        RemoveEvents();
    }

    void AddEvents()
    {
        Managers.Instance.GetManager<InputManager>()._serialReader.OnHardwareJoin.AddListener(DeviceJoinHandle);
        Managers.Instance.GetManager<InputManager>()._serialReader.OnHardwareLeave.AddListener(DeviceLeaveHandle);
    }

    void RemoveEvents()
    {
        Managers.Instance.GetManager<InputManager>()._serialReader.OnHardwareJoin.RemoveListener(DeviceJoinHandle);
        Managers.Instance.GetManager<InputManager>()._serialReader.OnHardwareLeave.RemoveListener(DeviceLeaveHandle);
    }

    public void DeviceJoinHandle(int Deviceindex)
    {
        // Agregar la tarea de invocar el evento a la cola
        lock (mainThreadQueue)
        {
            mainThreadQueue.Enqueue(() =>
            {
                if (Deviceindex >= 0 && Deviceindex < devicesEnter.Length)
                {
                    if (devicesEnter[Deviceindex] != null)
                    {
                        devicesEnter[Deviceindex]?.Invoke();
                    }
                }
            });
        }
    }

    public void DeviceLeaveHandle(int Deviceindex)
    {
        // Agregar la tarea de invocar el evento a la cola
        lock (mainThreadQueue)
        {
            mainThreadQueue.Enqueue(() =>
            {
                if (Deviceindex >= 0 && Deviceindex < devicesExit.Length)
                {
                    if (devicesExit[Deviceindex] != null)
                    {
                        devicesExit[Deviceindex]?.Invoke();
                        Debug.Log("Leave event invoked successfully on main thread.");
                    }
                }
            });
        }
    }

    private void Update()
    {
        // Ejecutar las órdenes pendientes en el hilo principal
        lock (mainThreadQueue)
        {
            while (mainThreadQueue.Count > 0)
            {
                var action = mainThreadQueue.Dequeue();
                action.Invoke();
            }
        }
    }
}
