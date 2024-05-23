using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ImageOscillator : MonoBehaviour
{
    public RectTransform imageRect; // RectTransform de la imagen

    public bool oscillateX = true; // Oscilar en el eje X
    public bool oscillateY = true; // Oscilar en el eje Y

    public float xMin = -100f; // Límite mínimo en el eje X
    public float xMax = 100f; // Límite máximo en el eje X
    public float yMin = -100f; // Límite mínimo en el eje Y
    public float yMax = 100f; // Límite máximo en el eje Y

    public float xSpeed = 1f; // Velocidad de oscilación en el eje X
    public float ySpeed = 1f; // Velocidad de oscilación en el eje Y

    private float xPos; // Posición actual en el eje X
    private float yPos; // Posición actual en el eje Y
    private float xDirection = 1f; // Dirección de movimiento en el eje X (1 = derecha, -1 = izquierda)
    private float yDirection = 1f; // Dirección de movimiento en el eje Y (1 = arriba, -1 = abajo)

    Vector2 InitPos;

    private void Start()
    {
        // Inicializamos las posiciones iniciales
        InitPos.x = xPos = imageRect.anchoredPosition.x;
        InitPos.y = yPos = imageRect.anchoredPosition.y;

    }

    private void Update()
    {
        if (oscillateX)
        {
            xPos += xSpeed * Time.deltaTime * xDirection;
            if (xPos >= xMax)
            {
                xPos = xMax;
                xDirection = -1f; // Cambiar dirección hacia la izquierda
            }
            else if (xPos <= xMin)
            {
                xPos = xMin;
                xDirection = 1f; // Cambiar dirección hacia la derecha
            }
            imageRect.anchoredPosition = new Vector2(xPos, imageRect.anchoredPosition.y);
        }

        // Oscilar en el eje Y
        if (oscillateY)
        {
            yPos += ySpeed * Time.deltaTime * yDirection;
            if (yPos >= yMax)
            {
                yPos = yMax;
                yDirection = -1f; // Cambiar dirección hacia abajo
            }
            else if (yPos <= yMin)
            {
                yPos = yMin;
                yDirection = 1f; // Cambiar dirección hacia arriba
            }
            imageRect.anchoredPosition = new Vector2(imageRect.anchoredPosition.x, yPos);
        }
    }
    public void ResetPosition(){
        imageRect.transform.localPosition = InitPos;
        xPos = yPos = 0;
    }
}