using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableArea : MonoBehaviour
{
    public bool noVotes;
    [Tooltip("Es la demora que tendrá el InteractableArea en recibir un nuevo voto")]
    public float SecondToRestart = 2f;
    public int[] ButonIdAcepted;
    public List<int> UsersVotes = new List<int>();
    [SerializeField] int _votesCount;

    public int VotesCount{
        get=>_votesCount;
        set
        {
            _votesCount = value;
            if(value!=0 && !noVotes){VerificarAccion();}
            if(noVotes){OnNoVotes?.Invoke();}
            MostrarJugadoresUI();
        }
    }
    [Tooltip("Acciones a ejecutar cuando recibe todos los votos o el supervoto")]
    public UnityEvent OnChooseThisArea;
    public UnityEvent OnNoVotes;

    [SerializeField] ScriptableUserSprite UserIcons;

    //ajustes visuales
    public float cellSize = 0.2f, cellSpace = 0.11f;
    GridLayoutGroup _gridLayoutGroup;
    float alto, ancho;
    bool _useOwnGrid;

    private void Awake() {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        if(_gridLayoutGroup==null){_gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();}else{_useOwnGrid = true;}
        OnChooseThisArea.AddListener(()=>{Invoke(nameof(ReinciarInteraction), SecondToRestart);});
    }
    private void Start() {
        UserIcons = Resources.Load<ScriptableUserSprite>("Scriptables/UsersSpriteScriptable");

        ancho = GetComponent<RectTransform>().rect.width;
        alto = GetComponent<RectTransform>().rect.height;
        if(!_useOwnGrid){
            _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = 4;
            _gridLayoutGroup.cellSize = new Vector2( ancho*cellSize, alto*cellSize );
            _gridLayoutGroup.spacing = new Vector2( ancho*cellSpace, alto*cellSpace );
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if(UserIcons!=null){transform.GetChild(i).gameObject.GetComponent<Image>().sprite = UserIcons.UserSprites[i];}
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void MostrarJugadoresUI(){
        for (int j = 0; j < transform.childCount; j++)
        {
            transform.GetChild(j).gameObject.SetActive(UsersVotes.Contains(j+1));
        }
    }
    void VerificarAccion(bool supervoto = false){
        if(UsersVotes!=null){
            int count = ((int)Managers.Instance.GetManager<InteractableManager>().Users.Count-1);
            //Debug.Log("AREA cuenta "+UsersVotes.Count +" de "+count+" por ende "+(VotesCount == ((int)Managers.Instance.GetManager<InteractableManager>().Users.Count-1) || supervoto)+" termina el conteo", transform);
            if( UsersVotes.Count == ((int)Managers.Instance.GetManager<InteractableManager>().Users.Count-1) || supervoto){ //si es un super voto o si todos votaron
                OnChooseThisArea?.Invoke();
                Managers.Instance.GetManager<InteractableManager>().ChangeInteractionMode(false);
                LimpiarArea();
            }
        }
    }
    public void LimpiarArea(){
        UsersVotes.Clear();
        //VotesCount = 0;
        MostrarJugadoresUI();

    }
    void ReinciarInteraction(){
        Managers.Instance.GetManager<InteractableManager>().ChangeInteractionMode(true); 
    }
    public void SuperVoto(ButtonData buttonData){
        VerificarAccion(true);
    }
    private void OnEnable() {
        //Debug.Log("Aparece");
        Invoke(nameof(RegistrarArea), 0.5f);
    }
    private void OnDisable() {
        Invoke(nameof(SuprimirArea), 0.5f);
    }
    void SuprimirArea(){
        LimpiarArea();
        if(Managers.Instance != null){
            InteractableManager interactableManager = Managers.Instance.GetManager<InteractableManager>();
            interactableManager.RemoveInteractionArea(this);
        }
    }
    void RegistrarArea(){
        InteractableManager interactableManager = Managers.Instance.GetManager<InteractableManager>();
        interactableManager.AddInteractionArea(this);
    }
}
