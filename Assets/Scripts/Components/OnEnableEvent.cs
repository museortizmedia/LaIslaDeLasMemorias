using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent OnEnableGO, OnDisableGO;
    private void OnEnable()
    {
        OnEnableGO?.Invoke();
    }
    private void OnDisable()
    {
        OnDisableGO?.Invoke();
    }
}
