using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public class TextBoxDialog
{
    public string Title;
    public string Content;
    public AudioClip Audio;
    public Sprite Personaje;
    public enum SpritePos {Izquierda,Centro,Derecha}
    public SpritePos SpritePosition = SpritePos.Centro;
}

[RequireComponent(typeof(AudioSource))]
public class TextBoxController : MonoBehaviour
{
    //boton seguir con chulo al terminar omitir tiempo
    public List<TextBoxDialog> Dialogs = new List<TextBoxDialog>();
    public TextMeshProUGUI TitleBox, ContentBox;
    public Image PersonajeBoxIzq, PersonajeBoxCen, PersonajeBoxDer;
    public Button NextBTN, BackBTN;
    public Sprite ArrowSprite, DoneSprite;
    [SerializeField] int _currentI = 0;
    public int CurrentI
    {
        get { return _currentI; }
        set
        {
            //StopAllCoroutines();
            _currentI = value >= Dialogs.Count ? 0 : value < 0 ? Dialogs.Count : value;
            NextBTN.gameObject.SetActive(_currentI < Dialogs.Count);
            CancelInvoke();
            NextBTN.interactable = false;
            if (_currentI == Dialogs.Count - 1) { IsLastSlide = true; } else { IsLastSlide = false; }
            BackBTN.gameObject.SetActive(_currentI > 0);
            SetContentBox();
        }
    }
    public float secondsAfterFinish = 2f;
    bool _isLastSlide;
    public bool IsLastSlide {
        get => _isLastSlide;
        set {
            _isLastSlide = value;
            NextBTN.GetComponent<Image>().sprite = value ? DoneSprite : ArrowSprite;
        }
    }
    public UnityEvent OnFinish;
    [SerializeField] AudioSource _as;

    private void Awake() {
        _as = GetComponent<AudioSource>();
        _as.playOnAwake = false;
    }

    private void Start()
    {
        NextBTN.onClick.AddListener(GoNext);
        BackBTN.onClick.AddListener(GoBack);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            GoNext();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            GoBack();
        }
    }

    private void OnEnable() {
        CurrentI = 0;
    }

    public void GoNext() {
        if (IsLastSlide) { OnFinish?.Invoke(); return; }
        CurrentI++;
    }
    public void GoBack() {
        CurrentI--;
    }
    void SetContentBox() {
        TitleBox.text = Dialogs[CurrentI].Title;
        ContentBox.text = Dialogs[CurrentI].Content;
        _as.clip = Dialogs[CurrentI].Audio;
        if (_as.clip != null) {
            _as.Play();
            Invoke(nameof(ActiveBtn), _as.clip.length);
        }
        else
        {
            Invoke(nameof(ActiveBtn), 2f);
        }
        

        Image[] images = new Image[] { PersonajeBoxIzq, PersonajeBoxCen, PersonajeBoxDer };
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
        images[(int)Dialogs[CurrentI].SpritePosition].sprite = Dialogs[CurrentI].Personaje;
        images[(int)Dialogs[CurrentI].SpritePosition].gameObject.SetActive(true);

    }

    void ActiveBtn()
    {
        NextBTN.interactable = true;
    }
}
