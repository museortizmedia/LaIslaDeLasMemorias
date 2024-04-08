using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public class TutorialCardController : MonoBehaviour
{
    [SerializeField] ExperienceController EC;
    [SerializeField] InteractableArea _interactableArea;
    [SerializeField] TextMeshProUGUI _contentBox;
    [SerializeField] string _contentBoxText = "Selecciona el botón que se indica en la imagen para realizar otro trazado";
    [SerializeField] List<int> buttonsOptions = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
    public int _currentButton = -1;
    [SerializeField] GameObject _cellGO;
    public UnityEvent OnFinishTutorial;

    //to do delete
    /*[Space(60)]
    [Header("CUIDADO! Control de depuradores")]
    [Tooltip("Si es true ejecutará el siguiente paso del tutorial")]
    public bool Manualcontrol;
    

    private void Update()
    {
        if(Manualcontrol){
            Manualcontrol=false;
            NextStep();
        }
    }*/
    //to do delete

    private void Start() {
        _contentBox.text = _contentBoxText;
    }

    private void OnEnable() {
        NextStep();
    }

    public void NextStep()
    {
        //verificacion si acabó el tutorial
        if(buttonsOptions.Count < 1){ OnFinishTutorial?.Invoke(); return; }

        //resetear tabla
        if(_currentButton!=-1){
            _cellGO.transform.GetChild(_currentButton).GetChild(0).gameObject.SetActive(false);
        }

        //escoger boton
        _currentButton = buttonsOptions[Random.Range(0, buttonsOptions.Count)];
        buttonsOptions.Remove(_currentButton);

        //definir boton en pantalla (gameEvents)
        _cellGO.transform.GetChild(_currentButton).GetChild(0).gameObject.SetActive(true);

        //definir ajustes de Area
        _interactableArea.ButonIdAcepted[0] = _currentButton+1; //se suma 1 para que sean rango de Interactable 1-12

        //forzar la activación de la interacción
        EC.Manager.GetManager<InteractableManager>().ChangeInteractionMode(true); 
    }

    public void ResetController(){
        buttonsOptions = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
    }

}
