using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Amb4Act1 : ExperienceController
{
    [SerializeField] List<CardControler> SecuenciaLavado = new();
    [SerializeField] List<ScriptableActivitiesData.ActivityData> Secuencia = new();
    List<ScriptableActivitiesData.ActivityData> NuevaSecuencia = new();
    [SerializeField] CardControler Q_Card, Q_Pregunta, Q_Area1, Q_Area2, Q_Area3;

    [Header("Otras referencias")]
    [SerializeField] GameObject SecuenciaGO;

    [SerializeField] GameObject FB_Positivo, FB_Negativo, FB_Absurdo, FB_Fin;

    List<int> _absurdIndex = new();

    int _correctArea = 0; //0-inactivo, 1-Area1, 2-Area2, 3-Area3
    ScriptableActivitiesData.ActivityData _cardInfo;

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
    public void MostrarSecuenciaLavadoDeManos()
    {
        SecuenciaGO.SetActive(true);
    }

    //guardar info de imagenes y textos de la secuencia
    void GenerarNuevaSecuencia()
    {
        //sumar absurdos según el porcentaje expuesto en datos
        NuevaSecuencia = new(ActivityData.Data);
        CombineListsRandomly(NuevaSecuencia, ActivityData.AbsurdData);

        Secuencia = new(NuevaSecuencia);
    }
    public void CombineListsRandomly<T>(List<T> Lista1, List<T> Lista2)
    {
        for (int i = 0; i < Lista2.Count; i++)
        {
            int absurdProbabiily = Random.Range(0, 100);
            if (absurdProbabiily < ActivityData.AbsurdPercent)
            {
                int random = Random.Range(0, Lista1.Count);
                Lista1.Insert(random, Lista2[i]);
                _absurdIndex.Add(random);
            }
        }
    }
    //definir contenido de lista en Controlador Question
    public void ShowQuestion()
    {
        //mostrar modo pregunta
        Q_Pregunta.gameObject.SetActive(true);
        Q_Area1.gameObject.SetActive(true);
        Q_Area2.gameObject.SetActive(true);
        Q_Area3.gameObject.SetActive(true);
        if(Q_Card.Type != CardType.ImageOnly) { Q_Card.SetCardType(CardType.ImageOnly); }

        //logica
        int index = Random.Range(0, Secuencia.Count);
        int index1 = Random.Range(0, NuevaSecuencia.Count); while (index == index1) { index1 = Random.Range(0, NuevaSecuencia.Count); }
        int index2 = Random.Range(0, NuevaSecuencia.Count); while (index1 == index2) { index2 = Random.Range(0, NuevaSecuencia.Count); }
        int index3 = Random.Range(0, NuevaSecuencia.Count); while (index2 == index3) { index3 = Random.Range(0, NuevaSecuencia.Count); }
        _correctArea = Random.Range(1, 4); //(1 inclusivo - 4(exclusivo))

        _cardInfo = Secuencia[index];
        Debug.Log(index + ", " + _cardInfo.Name + " " + index1 + " " + index2 + " " + index3 + " correcta: " + _correctArea);

        Q_Card.SetGameData(_cardInfo);
        Q_Pregunta.SetOnlyText("¿Cual es el nombre de este paso a realizar en el lavado de manos?");

        InteractableArea interactableArea1 = Q_Area1.GetComponentInChildren<InteractableArea>(),
            interactableArea2 = Q_Area2.GetComponentInChildren<InteractableArea>(),
            interactableArea3 = Q_Area3.GetComponentInChildren<InteractableArea>();
        //si es absurda
        if (_absurdIndex.Contains(NuevaSecuencia.IndexOf(_cardInfo)))
        {
            Debug.Log("Es una pregunta absurda...");
            Q_Area1.SetOnlyText(NuevaSecuencia[index1].Text);
            interactableArea1.OnChooseThisArea.RemoveAllListeners();
            interactableArea1.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            Q_Area2.SetOnlyText(NuevaSecuencia[index2].Text);
            interactableArea2.OnChooseThisArea.RemoveAllListeners();
            interactableArea2.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            Q_Area3.SetOnlyText(NuevaSecuencia[index3].Text);
            interactableArea3.OnChooseThisArea.RemoveAllListeners();
            interactableArea3.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            StartCoroutine(WaitAbsurdCorrutine());
            return;
        }
        else
        //si son normales
        {
            if (_correctArea == 1)
            {
                Q_Area1.SetOnlyText(_cardInfo.Text);
                interactableArea1.OnChooseThisArea.RemoveAllListeners();
                interactableArea1.OnChooseThisArea.AddListener(() => { PositiveFeedback(); });
                Q_Area2.SetOnlyText(NuevaSecuencia[index2].Text);
                interactableArea2.OnChooseThisArea.RemoveAllListeners();
                interactableArea2.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area3.SetOnlyText(NuevaSecuencia[index3].Text);
                interactableArea3.OnChooseThisArea.RemoveAllListeners();
                interactableArea3.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            }
            else if (_correctArea == 2)
            {
                Q_Area1.SetOnlyText(NuevaSecuencia[index1].Text);
                interactableArea1.OnChooseThisArea.RemoveAllListeners();
                interactableArea1.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area2.SetOnlyText(_cardInfo.Text);
                interactableArea2.OnChooseThisArea.RemoveAllListeners();
                interactableArea2.OnChooseThisArea.AddListener(() => { PositiveFeedback(); });
                Q_Area3.SetOnlyText(NuevaSecuencia[index3].Text);
                interactableArea3.OnChooseThisArea.RemoveAllListeners();
                interactableArea3.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
            }
            else if(_correctArea == 2)
            {
                Q_Area1.SetOnlyText(NuevaSecuencia[index1].Text);
                interactableArea1.OnChooseThisArea.RemoveAllListeners();
                interactableArea1.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area2.SetOnlyText(NuevaSecuencia[index2].Text);
                interactableArea2.OnChooseThisArea.RemoveAllListeners();
                interactableArea2.OnChooseThisArea.AddListener(() => { NegativeFeedback(); });
                Q_Area3.SetOnlyText(_cardInfo.Text);
                interactableArea3.OnChooseThisArea.RemoveAllListeners();
                interactableArea3.OnChooseThisArea.AddListener(() => { PositiveFeedback(); });
            }
        }
        ActiveInputManager(); //activamos la interaccion
        Q_Card.transform.parent.gameObject.SetActive(true);
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
    //si es correcta feedbackpositivo y pasar a siguiente de la lista
    void PositiveFeedback()
    {
        ShowAnswer();
        FB_Positivo.SetActive(true);
        Secuencia.Remove(_cardInfo);
    }
    //si es incorrecta feedback negativo
    void NegativeFeedback()
    {
        FB_Negativo.SetActive(true);
        //resetear areas
        ActiveInputManager();
        //si no hay mas en la lista feedbackfinal
    }
    public void ActiveInputManager()
    {
        Manager.GetManager<InteractableManager>().ChangeInteractionMode(true);
    }
    IEnumerator WaitAbsurdCorrutine()
    {
        yield return new WaitForSeconds(ActivityData.AbsurdWait);
        AbsurdFeedback();
    }
    public void ReinciarAbsurdo(){
        if (ActivityData.AbsurdData.Contains(_cardInfo)){
            StopAllCoroutines();
            StartCoroutine(WaitAbsurdCorrutine()); //reinciar corrutina
        }
    }
    void AbsurdFeedback()
    {
        FB_Absurdo.SetActive(true);
        NuevaSecuencia.Remove(_cardInfo);
        ShowAnswer();
    }

    public void Finalizar()
    {
        EndExperience(); //llamada obligatoria al finalizar
    }
}
