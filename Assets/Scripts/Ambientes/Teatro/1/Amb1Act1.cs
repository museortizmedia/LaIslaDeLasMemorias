using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Amb1Act1 : ExperienceController
{
    public List<int> buttonsUsedIndex = new List<int>();
    public TutorialCardController TableController = new TutorialCardController();

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

    void NextButton()
    {
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < TableController.buttons.Count; i++)
        {
            if (!buttonsUsedIndex.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count == 0)
        {
            Finalizar();
            return;
        }

        int randomIndex = Random.Range(0, availableIndexes.Count);
        int selectedIndex = availableIndexes[randomIndex];

        buttonsUsedIndex.Add(selectedIndex);
        TableController.ShowInfo(selectedIndex, "Selecciona el botón que se indica en la imagen para realizar otro trazado");
    }

    public void ShowFirstTutorial()
    {
        TableController.ShowInfo(2, "Selecciona el botón “E” de la mesa interactiva para pintar el primer fragmento de la pintura");
    }

    // TODO Delete this function when the InteractableArea Call the Events
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextButton();
        }
    }

}
