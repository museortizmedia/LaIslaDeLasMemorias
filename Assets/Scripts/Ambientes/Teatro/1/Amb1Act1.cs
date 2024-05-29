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

    [SerializeField] int _failCount;
    [SerializeField] List<int> _failList = new List<int>();

    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        OnEndTutorial.AddListener(()=>Finalizar());

        #if UNITY_EDITOR
        if(Manager.GetManager<InteractableManager>().Users.Count==1){IniciarCon(2);}
        #endif
    }
    public void EndTutorial(){
        OnEndTutorial?.Invoke();
    }
    void Finalizar()
    {
        EndExperience();
    }
    
    //Feedback Negativo (Button Events)
    public void SumarFail(ButtonData buttonData){
        int botonId = buttonData.ButtonId;
        int botonIndex = botonId-1;

        if(botonIndex != TableController._currentButton)
        {
            if(!_failList.Contains(botonIndex)){_failList.Add(buttonData.DeviceId);}

            if(_failList.Count == (Manager.GetManager<InteractableManager>().Users.Count-1)){
                ResetFailList();
                OnFailTutorial?.Invoke();
            }
        }
    }
    void ResetFailList(){
        _failList.Clear();
    }
}
