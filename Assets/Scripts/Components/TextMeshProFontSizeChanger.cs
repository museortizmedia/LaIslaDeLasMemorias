//#define DEBUGING_TEXT
using UnityEngine;
using TMPro;

[RequireComponent(typeof(GameEventListener))]
public class TextMeshProFontSizeChanger : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI
    public TextMeshProUGUI textMeshProUGUI;
    float _originalSize;
    private void Start() {
        _originalSize = textMeshProUGUI.fontSize;
    }

    // Método para cambiar el tamaño de la fuente
    public void SetFontSize(float newSize)
    {
        if (textMeshProUGUI != null)
        {
            // Desactivar el ajuste automático del tamaño de la fuente
            textMeshProUGUI.enableAutoSizing = false;

            // Establecer el nuevo tamaño de la fuente
            textMeshProUGUI.fontSize = _originalSize * newSize;
            #if DEBUGING_TEXT
            Debug.Log($"El tamaño de la fuente se ha cambiado a {newSize}");
            #endif
        }
        #if DEBUGING_TEXT
        else
        {
            Debug.LogError("TextMeshProUGUI no está asignado.");
        }
        #endif
    }
}