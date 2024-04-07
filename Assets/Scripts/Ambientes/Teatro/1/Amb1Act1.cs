using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Amb1Act1 : ExperienceController
{
    public List<int> buttonsUsedIndex = new List<int>();
    public TutorialCardController TableController;

    public UnityEvent OnEndTutorial;

    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        //Debug.Log(Manager.GetManager<InputManager>().name);
        IniciarCon(2);
        OnEndTutorial.AddListener(()=>Finalizar());
    }

    void Finalizar()
    {
        Debug.Log("Terminó la experiencia con éxito");
        // Desactiva esta línea para evitar que se cambie la escena
        EndExperience(); // llamada obligatoria al finalizar
    }

    public void NextButton()
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
            OnEndTutorial?.Invoke();
            return;
        }

        int randomIndex = Random.Range(0, availableIndexes.Count);
        int selectedIndex = availableIndexes[randomIndex];

        buttonsUsedIndex.Add(selectedIndex);
        TableController.ShowInfo(selectedIndex, "Selecciona el botón que se indica en la imagen para realizar otro trazado");
        Manager.GetManager<InteractableManager>().ChangeInteractionMode(true); //forzar la activación de la interacción
    }
}
