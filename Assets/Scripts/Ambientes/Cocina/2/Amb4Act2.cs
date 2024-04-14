using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Amb4Act2 : ExperienceController
{
    [SerializeField] List<CardControler> _cards = new();
    [SerializeField] List<InteractableArea> _interactables = new();
    public Sprite[] Terminados;
    public List<ScriptableActivitiesData.ActivityData> Receta;
    //public List<ScriptableActivitiesData.ActivityData> RecetaAbsurda;
    public GameObject CartasGO, InteractablesGO;
    public GameObject FB_Positivo, FB_Negativo, FB_Absurdo, FB_Fin;
    public Image PreparacionImage;
    int _flipedCard;
    
    public override void Start()
    {
        base.Start(); //llamada obligatoria al comenzar
        Debug.Log(Manager.GetManager<InputManager>().name);

        TraerInfoPreparaciones();
    }
    void TraerInfoPreparaciones(){

        Receta.Clear();
        int random = Random.Range(0, 4); //0-3
        //RecetaActual = random==0?"Salpicón":random==1?"Sandwich":random==2?"Panelada":"Sopa";
        PreparacionImage.sprite = Terminados[random];

        for (int i = 0; i < 4; i++)
        {            
            Receta.Add(ActivityData.Data[i+random*4]);
            Receta.Add(ActivityData.Data[i+random*4]);
        }
        //RecetaAbsurda = new(Receta);
        //CombinarListaAleatorio(RecetaAbsurda, ActivityData.AbsurdData);
        RevolverLista(Receta);


        for (int j = 0; j < _cards.Count; j++)
        {
            _cards[j].SetGameData(Receta[j]);
        }
    }
    public void GirarCartas(){
        for (int j = 0; j < _cards.Count; j++)
        {
            _cards[j].FlipCard();
        }
    }

    public void Comenzar(){
        CartasGO.SetActive(true);
        EsperaYRealiza(5f, ()=>{GirarCartas(); IniciarJuegoPareja();});
    }
    public void IniciarJuegoPareja(){

        _flipedCard = Random.Range(0, _cards.Count);
        _cards[_flipedCard].FlipCard();
        InteractablesGO.SetActive(true);

        InteractablesGO.transform.GetChild(_flipedCard>=4?_flipedCard+1:_flipedCard).GetComponent<InteractableArea>().ButonIdAcepted[0] = -1;
        ActiveInputManager();
    }

    public void ValidarCartaSeleccionada(CardControler cardSelected){

        if(cardSelected.Text == _cards[_flipedCard].Text){

            cardSelected.FlipCard();
            InteractablesGO.SetActive(false);
            EsperaYRealiza(2f, ()=>{                
                cardSelected.FlipCard();
                
                _cards[_flipedCard].SetCardType(CardType.Hiden);
                _cards[_flipedCard].Flipeable = false;
                _cards[_flipedCard].transform.GetChild(0).gameObject.SetActive(false);
                _cards.RemoveAt(_flipedCard);
                

                cardSelected.SetCardType(CardType.Hiden);
                cardSelected.Flipeable = false;
                cardSelected.transform.GetChild(0).gameObject.SetActive(false);
                _cards.Remove(cardSelected);

                InteractablesGO.transform.GetChild(cardSelected.transform.GetSiblingIndex()).GetComponent<InteractableArea>().ButonIdAcepted[0] = -1;

                _flipedCard=0;

                if(_cards.Count <= 0)
                {
                    Finalizar();
                    return;
                }
                PositiveFeedback();
            });
        }else{

            InteractablesGO.SetActive(false);

            cardSelected.FlipCard();
            EsperaYRealiza(2f, ()=>{                
                cardSelected.FlipCard();
                NegativeFeedback();
            });
        }
    }
    void PositiveFeedback(){
        _flipedCard = 0;
        FB_Positivo.SetActive(true);
        ActiveInputManager();
    }

    void NegativeFeedback()
    {
        FB_Negativo.SetActive(true);
        ActiveInputManager();
    }
    
    public void ToogleInteractables(bool state){
        InteractablesGO.SetActive(state); 
    }

    void Finalizar(){
        FB_Fin.SetActive(true);
    }
}
