using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Amb1Act1 : ExperienceController
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
    void LogicaDePrueba()
    {
        segundos++;
        if (segundos >= 5)
        {
            CancelInvoke();
            Debug.Log("Han pasado " + segundos + " segundos.");
            return;
        }
        Debug.Log("Han pasado " + segundos + " segundos.");
    }

    void Finalizar()
    {
        Debug.Log("Terminó la experiencia con éxito");
        // Desactiva esta línea para evitar que se cambie la escena
        // EndExperience(); // llamada obligatoria al finalizar
        ActivarWorkSpace(); // Llama a un método que activa el WorkSpace
    }

    void ActivarWorkSpace()
    {
        GameObject workSpace = GameObject.Find("WorkSpace"); // Encuentra el GameObject del Canvas
        if (workSpace != null)
        {
            workSpace.SetActive(true);
        }
        else
        {
            Debug.LogError("No se encontró el Canvas 'WorkSpace'");
        }
    }

}
