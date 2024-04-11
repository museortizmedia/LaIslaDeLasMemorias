using UnityEngine;
using UnityEngine.Events;

public class StartEvent : MonoBehaviour
{
    public UnityEvent OnStart;
    void Start()
    {
        OnStart?.Invoke();
    }
}
