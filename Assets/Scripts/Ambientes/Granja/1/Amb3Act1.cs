using System.Collections.Generic;
using UnityEngine;

public class Amb3Act1 : ExperienceController
{
    [SerializeField] List<ScriptableActivitiesData.ActivityData> animalesRound = new();
    int _currentAnimal = 0;
    [SerializeField] UI_FollowPath animalAnimations;
    [SerializeField] CardControler AnimalCard;
    [SerializeField] GameObject FB_Positive, FB_Negative, FB_Absurd, FB_End;
    [SerializeField] InteractableArea area1, area2, area3;
    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        Debug.Log(Manager.GetManager<InputManager>().name);

        //iniciamos la lista de los animales del Data
        animalesRound = new(ActivityData.Data); //hacemos una copia de los datos del scriptable
        CombinarListaAleatorio(animalesRound, ActivityData.AbsurdData); //combinamos nuestra copia de la lista con los datos de absurdos (no necesitamos shadowcopy porque el resultaod se guardará en animalRound (primer parametro))
        RevolverLista(animalesRound); //revolvemos aleatoriamente todos los animales
        DesactiveInputManager();
    }

    //iniciar recorrido
    ScriptableActivitiesData.ActivityData animalEscogido;
    public void StartEstampida(){
        area1.gameObject.SetActive(true);
        area2.gameObject.SetActive(true);
        area3.gameObject.SetActive(true);

        //seleccionamos el animal del incide de la lista
        animalEscogido = animalesRound[0];
        //verificamos si es absurdo, de ser así verificamos que se cumpla el porcentaje aleatorio (# aleatorio es menor que el porcentaje de absurdo de los datos de actividad)
        if(animalEscogido.IsAbsurd && Random.Range(0, 100)<ActivityData.AbsurdPercent)
        {
            animalesRound.RemoveAt(0); //pasamos al siguiente animal (no usamos el absurdo)
            StartEstampida(); //volvemos a usar este metodo (recursividad)
            return;
        }

        // Despues de verificar el animal a usar, asignamos sus valores a la carta
        AnimalCard.SetGameData(animalEscogido);
        AnimalCard.SetOnlyText(animalEscogido.Name);
       
        _currentCorrectAnimal = animalEscogido.Text == "Domestico"?1:animalEscogido.Text == "Salvaje"?2:3;
        AnimalCard.FlipCard();
    }

    //user interactions
    [SerializeField] int _currentCorrectAnimal = 0;
    //1- domesticos, 2-salvajes 3-acuaticos
    public void UserSelect(int AnimalType){
        if(_currentCorrectAnimal==0){return;} //sale si no hay animales seleccionados

        if(animalesRound.Count==1)
        {
            area1.gameObject.SetActive(false);
            area2.gameObject.SetActive(false);
            area3.gameObject.SetActive(false);
            FB_End.SetActive(true);
            FB_Positive.SetActive(false);
            FB_Negative.SetActive(false);
            FB_Absurd.SetActive(false);
            AnimalCard.FlipCard();
            return;
        }

        if(animalEscogido.IsAbsurd){
            area1.gameObject.SetActive(false);
            area2.gameObject.SetActive(false);
            area3.gameObject.SetActive(false);
            FB_Absurd.SetActive(true);
            return;
        }


        //verifica que el correcto sea el tocado
        if(_currentCorrectAnimal==AnimalType)
        {
            area1.gameObject.SetActive(false);
            area2.gameObject.SetActive(false);
            area3.gameObject.SetActive(false);
            _currentCorrectAnimal = 0;
            animalesRound.RemoveAt(0);
            ComeNewAnimal();
            FB_Positive.SetActive(true);

        }else{
            area1.gameObject.SetActive(false);
            area2.gameObject.SetActive(false);
            area3.gameObject.SetActive(false);
            FB_Negative.SetActive(true);
        }
    }

    public void ComeNewAnimal()
    {
        AnimalCard.FlipCard();
        StartEstampida();
    }

    public void Finalizar(){
        EndExperience();
    }
}
