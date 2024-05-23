using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amb3Act1 : ExperienceController
{
    [SerializeField] List<ScriptableActivitiesData.ActivityData> animalesRound = new();
    int _currentAnimal = 0;
    [SerializeField] UI_FollowPath animalAnimations;
    [SerializeField] CardControler AnimalCard;
    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        Debug.Log(Manager.GetManager<InputManager>().name);


        //iniciamos la lista de los animales del Data
        animalesRound = new(ActivityData.Data); //hacemos una copia de los datos del scriptable
        CombinarListaAleatorio(animalesRound, ActivityData.AbsurdData); //combinamos nuestra copia de la lista con los datos de absurdos (no necesitamos shadowcopy porque el resultaod se guardará en animalRound (primer parametro))
        RevolverLista(animalesRound); //revolvemos aleatoriamente todos los animales

    }

    //iniciar recorrido
    public void StartEstampida(){
        //seleccionamos el animal del incide de la lista
        ScriptableActivitiesData.ActivityData animalEscogido = animalesRound[_currentAnimal];
        //verificamos si es absurdo, de ser así verificamos que se cumpla el porcentaje aleatorio (# aleatorio es menor que el porcentaje de absurdo de los datos de actividad)
        if(animalEscogido.IsAbsurd && Random.Range(0, 100)<ActivityData.AbsurdPercent)
        {
            _currentAnimal++; //pasamos al siguiente animal (no usamos el absurdo)
            StartEstampida(); //volvemos a usar este metodo (recursividad)
            return;
        }

        // Despues de verificar el animal a usar, asignamos sus valores a la carta
        AnimalCard.SetGameData(animalEscogido);
        //animamos la llegada
        
    }

    //user interactions
    public void UserSelectDomestic(){
        //
    }

    public void UserSelectWild(){
        //
    }

    public void UserSelectAquatic(){
        //
    }

    void AnimationCardCome(){
        animalAnimations.WaypointIndex = 0;
        animalAnimations.OnStop.RemoveAllListeners();
        animalAnimations.OnStop.AddListener(()=>{
            //
        });
        animalAnimations.StartMoving();
    }
    /// <summary>
    /// anima la carta hacia un letrero
    /// </summary>
    /// <param name="letrero">1 - domestico, 2 - salvaje, 3 - acuatico</param>
    void AnimationCard(int letrero){
        animalAnimations.WaypointIndex = letrero;
        animalAnimations.OnStop.RemoveAllListeners();
        animalAnimations.OnStop.AddListener(()=>{
            //
        });
    }

}
