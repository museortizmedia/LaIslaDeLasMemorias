using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amb4Act2 : ExperienceController
{
    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        Debug.Log(Manager.GetManager<InputManager>().name);
    }


    //



    void Finalizar(){
        Debug.Log("Terminó la experiencia con éxito");
        EndExperience(); //llamada obligatoria al finalizar
    }
}
