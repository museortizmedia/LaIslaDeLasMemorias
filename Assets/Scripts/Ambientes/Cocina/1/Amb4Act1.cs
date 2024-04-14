using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Amb4Act1 : ExperienceController
{
    [SerializeField] List<CardControler> SecuenciaLavado = new();
    [SerializeField] List<ScriptableActivitiesData.ActivityData> Secuencia = new();
    [SerializeField] List<ScriptableActivitiesData.ActivityData> SecuenciaActual = new();
    [SerializeField] CardControler Q_Card, Q_Pregunta, Q_Area1, Q_Area2, Q_Area3;

    [Header("Otras referencias")]
    [SerializeField] GameObject SecuenciaLavadoGO;
    [SerializeField] GameObject SecuenciaLavadoGOEnd;

    [SerializeField] GameObject FB_Positivo, FB_Negativo, FB_Absurdo, FB_Fin;

    int _correctArea = 0; //0-inactivo, 1-Area1, 2-Area2, 3-Area3
    ScriptableActivitiesData.ActivityData _cardInfo;
    [SerializeField] int index;

    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        //Debug.Log(Manager.GetManager<InputManager>().name);
        SetearDataSecuenciaLavado();
        GenerarNuevaSecuencia();
    }
    void SetearDataSecuenciaLavado()
    {
        for (int i = 0; i < SecuenciaLavado.Count; i++)
        {
            SecuenciaLavado[i].SetGameData(ActivityData.Data[i]);
        }
    }
    //mostrar por x segundos la secuencia de lavado de manos
    public void MostrarSecuenciaLavadoDeManos(bool isLast = false)
    {
        if(isLast){SecuenciaLavadoGO.GetComponent<ActiveEvent>().enabled=false;}
        SecuenciaLavadoGO.SetActive(true);
    }

    //guardar info de imagenes y textos de la secuencia
    void GenerarNuevaSecuencia()
    {
        //sumar absurdos según el porcentaje expuesto en datos
        Secuencia = new(ActivityData.Data);
        CombinarListaAleatorio(Secuencia, ActivityData.AbsurdData);
        RevolverLista(Secuencia);

        SecuenciaActual = new(Secuencia);
    }
    
    //definir contenido de lista en Controlador Question
    public void ShowQuestion()
    {
        Transform QuestionGO = Q_Card.transform.parent;
        //si es el ultimo
        if(SecuenciaActual.Count==0){
            SecuenciaLavadoGO.SetActive(false);
            QuestionGO.gameObject.SetActive(false);
            FB_Fin.SetActive(true);
            SecuenciaLavadoGOEnd.SetActive(true);
            
            return;
        }

        //mostrar modo pregunta
        Q_Pregunta.gameObject.SetActive(true);
        Q_Area1.gameObject.SetActive(true);
        Q_Area2.gameObject.SetActive(true);
        Q_Area3.gameObject.SetActive(true);
        if(Q_Card.Type != CardType.ImageOnly) { Q_Card.SetCardType(CardType.ImageOnly); }
    
        //logica
        _cardInfo = SecuenciaActual[0];
        int index1 = Random.Range(0, Secuencia.Count); while (index1 == 0) { index1 = Random.Range(0, Secuencia.Count); }
        int index2 = Random.Range(0, Secuencia.Count); while (index2 == index || index2 == index1) { index2 = Random.Range(0, Secuencia.Count); }
        int index3 = Random.Range(0, Secuencia.Count); while (index3 == index || index3 == index1 || index3 == index2) { index3 = Random.Range(0, Secuencia.Count); }
        _correctArea = Random.Range(1, 4); //(1 inclusivo - 4(exclusivo))

        
        //Debug.Log(index1+Secuencia[index1].Name + " " + index2+Secuencia[index2].Name + " " + index3+Secuencia[index3].Name + " correcta: " + _correctArea);
        //Debug.Log("Respuesta correcta es: "+_correctArea);

        Q_Card.SetGameData(_cardInfo);
        Q_Pregunta.SetOnlyText("¿Cual es el nombre de este paso a realizar en el lavado de manos?");

        InteractableArea
        interactableArea1 = Q_Area1.GetComponentInChildren<InteractableArea>(),
        interactableArea2 = Q_Area2.GetComponentInChildren<InteractableArea>(),
        interactableArea3 = Q_Area3.GetComponentInChildren<InteractableArea>();

        //si es absurda
        if (_cardInfo.IsAbsurd)
        {
            Debug.Log("Es una pregunta absurda...");
            Q_Area1.SetOnlyText(Secuencia[index1].Text);
            interactableArea1.OnChooseThisArea.RemoveAllListeners();
            interactableArea1.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            Q_Area2.SetOnlyText(Secuencia[index2].Text);
            interactableArea2.OnChooseThisArea.RemoveAllListeners();
            interactableArea2.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            Q_Area3.SetOnlyText(Secuencia[index3].Text);
            interactableArea3.OnChooseThisArea.RemoveAllListeners();
            interactableArea3.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            StartAbsurdCorutine(()=>{AbsurdFeedback();});
        }else{
            //si son normales
            if (_correctArea == 1)
            {
                Q_Area1.SetOnlyText(_cardInfo.Text);
                interactableArea1.OnChooseThisArea.RemoveAllListeners();
                interactableArea1.OnChooseThisArea.AddListener(() => { PositiveFeedback(); });
                Q_Area2.SetOnlyText(Secuencia[index2].Text);
                interactableArea2.OnChooseThisArea.RemoveAllListeners();
                interactableArea2.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area3.SetOnlyText(Secuencia[index3].Text);
                interactableArea3.OnChooseThisArea.RemoveAllListeners();
                interactableArea3.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            }
            else if (_correctArea == 2)
            {
                Q_Area1.SetOnlyText(Secuencia[index1].Text);
                interactableArea1.OnChooseThisArea.RemoveAllListeners();
                interactableArea1.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area2.SetOnlyText(_cardInfo.Text);
                interactableArea2.OnChooseThisArea.RemoveAllListeners();
                interactableArea2.OnChooseThisArea.AddListener(() => { PositiveFeedback(); });
                Q_Area3.SetOnlyText(Secuencia[index3].Text);
                interactableArea3.OnChooseThisArea.RemoveAllListeners();
                interactableArea3.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            }
            else if(_correctArea == 3)
            {
                Q_Area1.SetOnlyText(Secuencia[index1].Text);
                interactableArea1.OnChooseThisArea.RemoveAllListeners();
                interactableArea1.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area2.SetOnlyText(Secuencia[index2].Text);
                interactableArea2.OnChooseThisArea.RemoveAllListeners();
                interactableArea2.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area3.SetOnlyText(_cardInfo.Text);
                interactableArea3.OnChooseThisArea.RemoveAllListeners();
                interactableArea3.OnChooseThisArea.AddListener(() => { PositiveFeedback(); });
            }
        }
        ActiveInputManager(); //activamos la interaccion
        QuestionGO.gameObject.SetActive(true);
    }

    public void ShowAnswer()
    {
        //mostrar solo tarjeta q_card y flipear
        Q_Card.FlipType = CardType.Badge;
        Q_Card.FlipCard();
        Q_Pregunta.gameObject.SetActive(false);
        Q_Area1.gameObject.SetActive(false);
        Q_Area2.gameObject.SetActive(false);
        Q_Area3.gameObject.SetActive(false);
    }
    void PositiveFeedback()
    {
        FB_Positivo.SetActive(true);
        SecuenciaActual.RemoveAt(0);
        ShowAnswer();
        
        StopAbsurdCorutine();     
    }
    void NegativeFeedback()
    {
        FB_Negativo.SetActive(true);
        ActiveInputManager();
        ReinciarAbsurdo();
    }
    
    public void ReinciarAbsurdo(){
        if (ActivityData.AbsurdData.Contains(_cardInfo)){
            Debug.Log("reinciando tiempo");
            RestartAbsurdCorutine(()=>{AbsurdFeedback();});
        }
    }
    void AbsurdFeedback()
    {
        FB_Absurdo.SetActive(true);
        SecuenciaActual.RemoveAt(0);
        ShowAnswer();
    }

    public void Finalizar()
    {
        EndExperience(); //llamada obligatoria al finalizar
    }
}
