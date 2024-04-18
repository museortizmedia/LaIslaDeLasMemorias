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
    Badge,
    TextOnly,
    Hiden
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
    [Header("Sound")]
    [SerializeField] bool _isMuted;

    Image _imageCardSprite;
    AudioSource _audioSource;

    private void Start() {
        PrivateComponents();
        Inicializar();
    }

    void PrivateComponents(){
        _imageCardSprite = _imageCard.GetComponentInChildren<Image>();
        _audioSource = GetComponent<AudioSource>();
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
            case CardType.TextOnly:
                _badge.SetActive(false);
                _imageCard.SetActive(false);
                _textCard.gameObject.SetActive(true);
                //set
                _textCard.text = Text;
            break;
            case CardType.Hiden:
                _badge.SetActive(false);
                _imageCard.SetActive(false);
                _textCard.gameObject.SetActive(false);
            break;
        }
        _imageCardSprite.sprite = Image;
        _audioSource.clip = Sound;
        _textCard.fontSize = FontSize;
    }

    /*private void OnEnable() {
        if(_audioSource!=null && _audioSource.clip != null && !_isMuted) { _audioSource.Play(); }
    }

    private void OnDisable() {
        _audioSource.Stop();
    }*/

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
            elapsedRotationTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedRotationTime / (_flipDuration / 2));
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }
        FlipCB?.Invoke();

        // Reajuste de la rotación
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
        elapsedRotationTime = 0f;

        while (elapsedRotationTime < (_flipDuration / 2))
        {
            elapsedRotationTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedRotationTime / (_flipDuration / 2));
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation; //asegurar rotación
    }
    public void SetCardType(CardType type){
        Type=type;
        Inicializar();
    }
    public void SetGameData(ScriptableActivitiesData.ActivityData Data){
        Text = Data.Text;
        Image = Data.Imagen;
        Sound = Data.Sound;
        BadgeNumber = Data.BadgeInt;
        Inicializar();
    }
    public void SetOnlyText(string Data){
        Text = Data;
        Inicializar();
    }
    public void SetOnlyImage(Sprite Data){
        Image = Data;
        Inicializar();
    }
    public void SetOnlySound(AudioClip Data){
        Sound = Data;
        Inicializar();
    }
    public void SetOnlyBadgeText(int Data){
        BadgeNumber = Data;
        Inicializar();
    }
}

#if UNITY_EDITOR
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
#endif
