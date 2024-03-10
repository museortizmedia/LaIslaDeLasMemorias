using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Define una clase para el ScriptableObject
[CreateAssetMenu(fileName = "ScriptableActivitiesInfo", menuName = "ScriptableObject/ActivitiesInfo", order = 51)]
public class ScriptableActivitiesInfo : ScriptableObject
{
    [Serializable]
    public struct Ambiente{
        [Serializable]
        public struct Actividad{
            public string _name;
            public string _desc;
        }
        public string _name;
        public List<Actividad> _activitiesNames;
    }
    public List<Ambiente> Ambientes;
}
