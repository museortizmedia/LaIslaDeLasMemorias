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

    public void ContenidoDePrueba(){
        InvokeRepeating(nameof(LogicaDePrueba), 0, 1);
    }
    int segundos;
    void LogicaDePrueba(){
        segundos++;
        if(segundos>=5){CancelInvoke(); Finalizar(); return;}
        Debug.Log("Han pasado "+segundos+" segundos.");
    }

    void Finalizar(){
        Debug.Log("Terminó la experiencia con éxito");
        EndExperience(); //llamada obligatoria al finalizar
    }
}
