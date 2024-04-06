using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowForParts : MonoBehaviour
{
    // For use this component, must have React Transform component
    private float originalWidth, height;
    public int partsInX = 1;
    private int currentPartsInX = 0;

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        originalWidth = - rectTransform.rect.width;
        height = rectTransform.rect.height;

        rectTransform.offsetMax = new Vector2(originalWidth, 0);
    }
    public void ShowOnePartMore()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (currentPartsInX < partsInX)
        {
            currentPartsInX++;
            rectTransform.offsetMax = new Vector2(originalWidth - (originalWidth * currentPartsInX) / partsInX, 0);
        }
    }

    public void ShowAllParts()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        currentPartsInX = partsInX;
        rectTransform.offsetMax = new Vector2(0, 0);
    }
}
