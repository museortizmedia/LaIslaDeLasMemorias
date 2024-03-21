using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInPlay : MonoBehaviour
{
    [Tooltip("Si es true encenderá el GO, si es false lo apagará en el arranque")]
    public bool CanStayActiveGO;
    private void Awake() {
        gameObject.SetActive(CanStayActiveGO);
    }
}
