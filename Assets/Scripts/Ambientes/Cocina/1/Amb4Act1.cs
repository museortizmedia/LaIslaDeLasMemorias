using System.Collections.Generic;
using UnityEngine;

public class Amb4Act1 : ExperienceController
{
    [SerializeField] List<CardControler> SecuenciaLavado = new();
    [SerializeField] List<ScriptableActivitiesData.ActivityData> NuevaSecuencia = new();

    [Header("Otras referencias")]
    [SerializeField] GameObject SecuenciaGO;
    [SerializeField] CardControler Q_Card, Q_Pregunta, Q_Area1, Q_Area2, Q_Area3;

    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        //Debug.Log(Manager.GetManager<InputManager>().name);
        SetearDataSecuenciaLavado();
        GenerarNuevaSecuencia();
    }
    void SetearDataSecuenciaLavado(){
        for (int i = 0; i < SecuenciaLavado.Count; i++)
        {
            SecuenciaLavado[i].SetGameData(ActivityData.Data[i]);
        }
    }
    //mostrar por x segundos la secuencia de lavado de manos
    public void MostrarSecuenciaLavadoDeManos(){
        SecuenciaGO.SetActive(true);
    }
    
    //guardar info de imagenes y textos de la secuencia
    void GenerarNuevaSecuencia(){
        NuevaSecuencia = new(ActivityData.Data);
        //sumar absurdos según el porcentaje expuesto en datos
        CombineListsRandomly(NuevaSecuencia, ActivityData.AbsurdData);
    }
    public void CombineListsRandomly<T>(List<T> Lista1, List<T> Lista2)
    {
        for (int i = 0; i < Lista2.Count; i++)
        {
            int absurdProbabiily = Random.Range(0, 100);
            Debug.Log(absurdProbabiily);
            if(absurdProbabiily < ActivityData.AbsurdPercent)
            {
                int random = Random.Range(0, Lista1.Count);
                Lista1.Insert(random, Lista2[i]);
            }
        }
    }
    //definir contenido de lista en Controlador Question
    public void ShowQuestion(){
        int index = Random.Range(0, NuevaSecuencia.Count);
        int index1 = Random.Range(0, NuevaSecuencia.Count);
        int index2 = Random.Range(0, NuevaSecuencia.Count);
        int index3 = Random.Range(0, NuevaSecuencia.Count);        
        int CorrectArea = Random.Range(1, 3);
        ScriptableActivitiesData.ActivityData CardInfo = NuevaSecuencia[index];
        Q_Card.SetGameData(CardInfo);
        Q_Pregunta.SetGameData(CardInfo);
        Q_Area1.Text = CorrectArea==1?CardInfo.Text:NuevaSecuencia[index1].Text;
        Q_Area2.Text = CorrectArea==2?CardInfo.Text:NuevaSecuencia[index2].Text;
        Q_Area3.Text = CorrectArea==3?CardInfo.Text:NuevaSecuencia[index3].Text;
        NuevaSecuencia.Remove(CardInfo);
    }
    //si es correcta feedbackpositivo y pasar a siguiente de la lista
    void PositiveFeedback(){
        //
    }
    //si es incorrecta feedback negativo
    void NegativeFeedback(){
        //
        //resetear areas
        //si no hay mas en la lista feedbackfinal
    }    

    void Finalizar(){
        EndExperience(); //llamada obligatoria al finalizar
    }
}
