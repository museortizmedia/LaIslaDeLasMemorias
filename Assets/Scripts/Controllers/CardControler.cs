using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System.Collections;
using System;


public enum CardType {
    Card,
    ImageOnly,
    Badge
}

[RequireComponent(typeof(AudioSource))]
public class CardControler : MonoBehaviour
{
    public CardType Type;
    [Header("References")]
    [SerializeField] GameObject _badge;
    [SerializeField] GameObject _imageCard;
    [SerializeField] TextMeshProUGUI _textCard;
    [Header("Data")]
    [TextArea]
    public string Text;
    public Sprite Image;
    public AudioClip Sound;
    public int BadgeNumber = 0;
    public float FontSize = 15f;
    [Header("Flip")]
    public bool Flipeable;
    public CardType FlipType;
    [SerializeField]float _flipDuration = 1f;

    Image _imageCardSprite;
    AudioSource _audioSource;

    private void Start() {
        PrivateComponents();
        Inicializar();
    }

    void PrivateComponents(){
        _imageCardSprite = _imageCard.GetComponentInChildren<Image>();
        _audioSource = GetComponent<AudioSource>();
        _textCard.fontSize = FontSize;
    }

    public void Inicializar() {
        if(_imageCardSprite == null || _audioSource == null){
            PrivateComponents();
        }

        switch (Type) {
            default:
            case CardType.Card:
                _badge.SetActive(false);
                _imageCard.SetActive(true);
                _textCard.gameObject.SetActive(true);
                //set
                _textCard.text = Text;
            break;
            case CardType.ImageOnly:
                _badge.SetActive(false);
                _imageCard.SetActive(true);
                _textCard.gameObject.SetActive(false);
                //set
            break;
            case CardType.Badge:
                _badge.SetActive(true);
                _imageCard.SetActive(true);
                _textCard.gameObject.SetActive(true);
                //set
                
                _textCard.text = Text;
                _badge.GetComponentInChildren<TextMeshProUGUI>(true).text = BadgeNumber.ToString();
            break;
        }
        _imageCardSprite.sprite = Image;
        _audioSource.clip = Sound;
    }

    public void FlipCard(){
        if(Flipeable){
            StartCoroutine(RotateCard(()=>{
                (FlipType, Type) = (Type, FlipType); //swype values
                Inicializar();
            }) );
        }
    }
    IEnumerator RotateCard(Action FlipCB)
    {
        float elapsedRotationTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 90f, 0f); // Rotación deseada (90 grados en el eje Y)

        while (elapsedRotationTime < (_flipDuration / 2))
        {
            // Incrementar el tiempo transcurrido
            elapsedRotationTime += Time.deltaTime;

            // Calcular el progreso de la animación (entre 0 y 1)
            float t = Mathf.Clamp01(elapsedRotationTime / (_flipDuration / 2));

            // Interpolar la rotación desde la rotación inicial hasta la rotación objetivo
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Esperar un frame antes de continuar la rotación
            yield return null;
        }

        // Llamada a la acción una vez que la mitad de la rotación se ha completado
        FlipCB?.Invoke();

        // Reajuste de la rotación
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        elapsedRotationTime = 0f;

        while (elapsedRotationTime < (_flipDuration / 2))
        {
            // Incrementar el tiempo transcurrido
            elapsedRotationTime += Time.deltaTime;

            // Calcular el progreso de la animación (entre 0 y 1)
            float t = Mathf.Clamp01(elapsedRotationTime / (_flipDuration / 2));

            // Interpolar la rotación desde la rotación inicial hasta la rotación objetivo
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Esperar un frame antes de continuar la rotación
            yield return null;
        }

        // Asegurar que la rotación llegue exactamente a la rotación original
        transform.rotation = targetRotation;
    }
}

[CustomEditor(typeof(CardControler))]
public class MyScriptEditor : Editor {
    public override void OnInspectorGUI() {
        CardControler myScript = (CardControler)target;

        DrawDefaultInspector();

        if (GUI.changed) {
            myScript.Inicializar();
        }
    }
}
