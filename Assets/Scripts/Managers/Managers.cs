using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    [SerializeField] List<Component> _managers = new List<Component>();
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
            if (child.GetType() != typeof(Transform) && child.GetType() != typeof(Managers) )
            {
                _managers.Add(child);
            }
        }
    }
}
