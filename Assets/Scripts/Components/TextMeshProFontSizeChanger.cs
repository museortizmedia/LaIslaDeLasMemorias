using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

[RequireComponent(typeof(GameEventListener))]
public class TextMeshProFontSizeChanger : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI
    public TextMeshProUGUI textMeshProUGUI;
    GameEventListener gameEventListener;
    [SerializeField, TextArea] string LABEL = "↓ Asigne en el Event el FontSizer y en ResizeResponse le método dinamico SetFontSize";

    private void Start() {
        gameEventListener = GetComponent<GameEventListener>();
        gameEventListener.Event = (GameEvent)Resources.Load("Scriptables/GameEvents/FontSizer");
    }

    // Método para cambiar el tamaño de la fuente
    public void SetFontSize(float newSize)
    {
        if (textMeshProUGUI != null)
        {
            // Desactivar el ajuste automático del tamaño de la fuente
            textMeshProUGUI.enableAutoSizing = false;

            // Establecer el nuevo tamaño de la fuente
            textMeshProUGUI.fontSize = newSize;

            Debug.Log($"El tamaño de la fuente se ha cambiado a {newSize}");
        }
        else
        {
            Debug.LogError("TextMeshProUGUI no está asignado.");
        }
    }
}