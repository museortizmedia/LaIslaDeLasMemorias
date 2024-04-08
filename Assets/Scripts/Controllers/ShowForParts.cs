using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowForParts : MonoBehaviour
{
    
    public int partsInX = 12;
    [SerializeField] private int currentPartsInX = 0;
    public UnityEvent OnChange;

    RectTransform _rectTransform;
    private float originalWidth, height;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        originalWidth = - _rectTransform.rect.width;
        height = _rectTransform.rect.height;

        _rectTransform.offsetMax = new Vector2(0, 0); //iniciar siempre viendose
    }
    public void ShowOnePartMore()
    {
        if (currentPartsInX < partsInX)
        {
            currentPartsInX++;
            _rectTransform.offsetMax = new Vector2(originalWidth - (originalWidth * currentPartsInX) / partsInX, 0);
            OnChange?.Invoke();
        }
    }

    public void ShowAllParts()
    {
        currentPartsInX = partsInX;
        _rectTransform.offsetMax = new Vector2(0, 0);
        OnChange?.Invoke();
    }
    public void ShowAnyPart(){
        currentPartsInX=0;
        _rectTransform.offsetMax = new Vector2(originalWidth, 0);
    }
}