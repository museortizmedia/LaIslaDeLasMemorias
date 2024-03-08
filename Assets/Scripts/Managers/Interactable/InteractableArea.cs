using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableArea : MonoBehaviour
{
    public int[] ButonIdAcepted;
    GridLayoutGroup _gridLayoutGroup;
    public int votesCount;
    private void Awake() {
        if(_gridLayoutGroup==null){_gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();}
    }
    private void Start() {
        
        //_gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        //_gridLayoutGroup.constraint =GridLayoutGroup.Constraint.FixedColumnCount;
        //_gridLayoutGroup.constraintCount = 2;
    }
}
