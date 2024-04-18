using System;
using System.Collections.Generic;
using UnityEngine;

// Define una clase para el ScriptableObject
[CreateAssetMenu(fileName = "IntroMensajesScriptable", menuName = "ScriptableObject/IntroMensajes", order = 51)]
public class ScriptableIntroMensajes : ScriptableObject
{
    [Serializable]
    public struct DialogoIntro{
        public string _name;
        public List<DialogoActividad> DialogoActividades;        
    }
    [Serializable]
    public struct DialogoActividad
    {
        public string _name;
        public List<TextBoxDialog> Dialogo;
    }
    public List<DialogoIntro> TodosLosDialogos;
    
}
