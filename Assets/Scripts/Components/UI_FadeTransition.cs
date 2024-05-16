using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MuseCoderLibrary{
/// <summary>
/// Hará una fundido a negro en pantalla creando un nuevo canvas. Realiza acciones antes, durante y al finalizar.
/// Uselo añadiendo el componente seguido del metodo Iniciar:
/// gameObject.AddComponent<UI_FadeTransition>().Iniciar(()=>{},()=>{},()=>{Application.Quit();});
/// </summary>
public class UI_FadeTransition : MonoBehaviour
{
    public GameObject CanvaFade;
    public Image image;
    public bool onUseOnly;
    public float fadeDuration = 1f;
    public float intermidateDuration = .5f;

    public UnityEvent OnBefore, OnMiddle, OnAfter;
    /// <summary>
    /// Genera una transición en negro de fade
    /// </summary>
    /// <param name="middle">función anónima que se ejecuta en medio de la transición</param>
    /// <param name="before">OPCIONAL: función anónima que se ejecuta antes de comenzar la transición</param>
    /// <param name="after">OPCIONAL: función anónima que se ejecuta después de terminar la transición</param>
    /// <param name="duracionFade">OPCIONAL: float del tiempo de transición, por defecto es 1 segundo de aparición y 1 segundo de desparición</param>
    /// <param name="duracionNegro">OPCIONAL: float del tiempo que durará en negro la transición, por defecto es medio segundo.</param>
    public void  Iniciar(UnityAction middle, UnityAction before = null, UnityAction after = null, float duracionFade = 1f, float duracionNegro = .5f ){
        fadeDuration = duracionFade;
        intermidateDuration = duracionNegro;

        OnBefore = new UnityEvent();
        OnMiddle = new UnityEvent();
        OnAfter = new UnityEvent();

        if(before!=null){OnBefore?.AddListener(before);}
        OnMiddle?.AddListener(middle);
        if(after!=null){OnAfter?.AddListener(after);}

        showTransition();
    }

    void Start()
    {
        if(onUseOnly){showTransition();}
    }

    private void OnDisable() {
        Destroy(CanvaFade);
    }

    public void showTransition(){
        showFade();
    }

    void showFade(){
        StartCoroutine(FadeInOut());
    }

    bool crearCanvas(){
        // Crear un nuevo objeto Canvas
        CanvaFade = new GameObject("Canvas");
        CanvaFade.name = "UITRANSITION";
        Canvas canvas = CanvaFade.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingLayerID = SortingLayer.NameToID("UI");
        canvas.sortingOrder = 999;

        // Agregar un objeto de imagen al canvas
        GameObject imageObject = new GameObject("Image");
        image = imageObject.AddComponent<Image>();
        image.color = new Color(0,0,0,0);
        imageObject.transform.SetParent(canvas.transform, false);

        // Obtener la resolución de pantalla
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Establecer el tamaño y la posición de la imagen para que sea de pantalla completa
        RectTransform imageTransform = image.GetComponent<RectTransform>();
        imageTransform.anchorMin = new Vector2(0, 0);
        imageTransform.anchorMax = new Vector2(1, 1);
        imageTransform.sizeDelta = new Vector2(screenWidth, screenHeight);
        return true;
    }


    IEnumerator FadeInOut()
    {
        crearCanvas();
        OnBefore?.Invoke();
        // Desvanecer la imagen de transparente (alpha = 0) a completamente visible (alpha = 1)
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }

        // Esperar unos segundos
        OnMiddle?.Invoke();
        yield return new WaitForSeconds(intermidateDuration);

        // Desvanecer la imagen de visible (alpha = 1) a transparente (alpha = 0)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
        OnAfter?.Invoke();
        Destroy(CanvaFade);
        if(onUseOnly){Destroy(this);}
    }
}
}
