using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickTrigger : MonoBehaviour
{
    [Header("Read")]
    [SerializeField] bool _active;
    public bool Active{get=>_active; set=>_active = value;}
    public UnityEvent OnClic;
    void Update()
    {
        if(Active && Input.GetMouseButtonDown(0)){
            OnClic?.Invoke();
        }
    }
}
