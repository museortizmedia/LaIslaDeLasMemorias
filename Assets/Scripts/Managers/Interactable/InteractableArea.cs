using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableArea : MonoBehaviour
{
    public int[] ButonIdAcepted;
    public List<int> UsersVotes;
    [SerializeField] int _votesCount;
    public int VotesCount{
        get=>_votesCount;
        set
        {
            _votesCount = value;
            MostrarJugadoresUI();
        }
    }
    [SerializeField] ScriptableUserSprite UserIcons;

    //ajustes visuales
    public float cellSize = 0.2f, cellSpace = 0.11f;
    GridLayoutGroup _gridLayoutGroup;
    float alto, ancho;

    private void Awake() {
        if(_gridLayoutGroup==null){_gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();}
    }
    private void Start() {
        UserIcons = Resources.Load<ScriptableUserSprite>("Scriptables/UsersSpriteScriptable");

        ancho = GetComponent<RectTransform>().rect.width;
        alto = GetComponent<RectTransform>().rect.height;
        
        _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        _gridLayoutGroup.constraint =GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = 4;
        _gridLayoutGroup.cellSize = new Vector2( ancho*cellSize, alto*cellSize );
        _gridLayoutGroup.spacing = new Vector2( ancho*cellSpace, alto*cellSpace );

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
    private void OnEnable() {
        Invoke(nameof(RegistrarArea), 0.5f);
    }
    private void OnDisable() {
        InteractableManager interactableManager = (InteractableManager)Managers.Instance._managers[2];
        interactableManager.AddInteractionRemove(this);
    }
    void RegistrarArea(){
        InteractableManager interactableManager = (InteractableManager)Managers.Instance._managers[2];
        interactableManager.AddInteractionArea(this);
    }
}
