using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteInUnityEditor : MonoBehaviour
{
    public UnityEvent OnStartEvent, OnAwakeEvent, OnEnableEvent, OnUpdateEvent, OnDisableEvent, OnDestroyEvent;
    void Start()
    {
        #if UNITY_EDITOR
        OnStartEvent?.Invoke();
        #endif
    }
    private void Awake()
    {
        #if UNITY_EDITOR
        OnAwakeEvent?.Invoke();
        #endif
    }
    private void OnEnable()
    {
        #if UNITY_EDITOR
        OnEnableEvent?.Invoke();
        #endif
    }

    void Update()
    {
        #if UNITY_EDITOR
        OnUpdateEvent?.Invoke();
        #endif
    }
    private void OnDisable()
    {
        #if UNITY_EDITOR
        OnDisableEvent?.Invoke();
        #endif
    }
    private void OnDestroy()
    {
        #if UNITY_EDITOR
        OnDestroyEvent?.Invoke();
        #endif
    }
}
