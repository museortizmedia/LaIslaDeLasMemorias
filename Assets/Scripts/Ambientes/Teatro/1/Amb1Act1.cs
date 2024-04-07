using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Amb1Act1 : ExperienceController
{
    //public List<int> buttonsUsedIndex = new List<int>();
    public TutorialCardController TableController;

    public UnityEvent OnFailTutorial, OnEndTutorial;

    int _failCount;

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

    public void SumarFail(){
        _failCount++;
        if(_failCount == (Manager.GetManager<InteractableManager>().Users.Count-1)){
            ResetFailCount();
            OnFailTutorial?.Invoke();
        }
    }
    void ResetFailCount(){
        _failCount = 0;
    }

    public void EndTutorial(){
        OnEndTutorial?.Invoke();
    }
}
