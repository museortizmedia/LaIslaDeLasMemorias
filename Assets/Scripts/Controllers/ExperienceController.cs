using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public Managers Manager;

    private void Start()
    {
        Manager = Managers.Instance;
    }
}
