using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class SensibilityManager : MonoBehaviour, IManager
{
    public bool isActive;
    [Header("Vibration and Resizer")]
    [Range(0.0f, 10.0f)] public float VibrationSensibility;
    [SerializeField] GameEvent _resizerEvent;

    [Header("Color")]
    public int ActiveEfectIndex;
    [Header("Volume")]
    [SerializeField] AudioMixer audioMix;
    [SerializeField] AudioMixerGroup sfxGroup, musicGroup;

    GameObject _vibrationControlPrefab, _colorControlPrefab, _volumeControlPrefab;
    GameObject _vibrationControl, _colorControl, _volumeControl;
    
    void Start()
    {
        CrearControlHUD();
    }
    void Update() {
        if(isActive){
            if(Input.GetKeyDown(KeyCode.J)){
                //Open Vibration Control
                Debug.Log("vibration");
                ModuleSetActive(!_vibrationControl.activeSelf, _vibrationControl);
                VibrationConfig();
            }else if(Input.GetKeyDown(KeyCode.K)){
                //Open Color Control
                Debug.Log("color");
                ModuleSetActive(!_colorControl.activeSelf, _colorControl);
                ColorConfig();
            }else if(Input.GetKeyDown(KeyCode.L)){
                //Open Volume Control
                Debug.Log("volume");
                ModuleSetActive(!_volumeControl.activeSelf, _volumeControl);
                VolumeConfig();
            }
        }
    }

    public void CrearControlHUD()
    {
        //crearHUD
        _vibrationControlPrefab = Resources.Load<GameObject>("Prefabs/VibrationControlPrefab");
        _colorControlPrefab = Resources.Load<GameObject>("Prefabs/ColorControlPrefab");
        _volumeControlPrefab = Resources.Load<GameObject>("Prefabs/VolumeControlPrefab");
        //vibration
        _vibrationControl = Instantiate(_vibrationControlPrefab, transform);
        //color
        _colorControl = Instantiate(_colorControlPrefab, transform);
        
        //volume
        _volumeControl = Instantiate(_volumeControlPrefab, transform);

        _vibrationControl.SetActive(false);
        _colorControl.SetActive(false);
        _volumeControl.SetActive(false);
    }

    public void ModuleSetActive(bool show, GameObject module){
        module.SetActive(show);
    }
    void VibrationConfig(){
        Slider[] _sliders = _vibrationControl.GetComponentsInChildren<Slider>();
        //slider vibration
        _sliders[0].onValueChanged.RemoveAllListeners();
        _sliders[0].onValueChanged.AddListener((float sliderValue)=>{
            SetVibrationPower(sliderValue);
        });
        //slider resizer
        _sliders[1].onValueChanged.RemoveAllListeners();
        _sliders[1].onValueChanged.AddListener((float sliderValue)=>{
            _resizerEvent.ResizeRaise(sliderValue);
            //Debug.Log($"cambiando en factor de {sliderValue}");
        });
        _resizerEvent.ResizeRaise(_sliders[1].value);
    }
    void ColorConfig(){
        /*Button[] botones = _colorControl.GetComponentsInChildren<Button>();
        botones[0].onClick.RemoveAllListeners();
        botones[1].onClick.RemoveAllListeners();
        botones[0].onClick.AddListener(()=>{SetCameraEfect();});
        botones[1].onClick.AddListener(() =>{SetCameraEfect();});*/
        TMP_Dropdown _tmpDropdown = _colorControl.GetComponentInChildren<TMP_Dropdown>(true);
        _tmpDropdown.onValueChanged.RemoveAllListeners();
        _tmpDropdown.onValueChanged.AddListener((int value)=>{SetCameraEfect(value);});

    }

    void VolumeConfig(){
        Slider[] _sliders = _volumeControl.GetComponentsInChildren<Slider>(true);
        _sliders[0].onValueChanged.RemoveAllListeners();
        _sliders[1].onValueChanged.RemoveAllListeners();
        _sliders[2].onValueChanged.RemoveAllListeners();
        _sliders[2].onValueChanged.AddListener((float sliderValue)=>{SetAudioVolume(sliderValue);});
        _sliders[1].onValueChanged.AddListener((float sliderValue)=>{SetAudioVolume(sliderValue, "MusicVolume");});
        _sliders[0].onValueChanged.AddListener((float sliderValue)=>{SetAudioVolume(sliderValue, "SfxVolume");});
    }

    //VibrationControl
    public void SetVibrationPower(float value){
        Managers manager = GetComponentInParent<Managers>();
        SerialReader serialReader = (SerialReader)manager._managers[1];
        serialReader.SendSerialPortData(SerialReader.CerebroComds.ConfigVibration+"["+value+"]");
    }

    //ColorControl
    void SetCameraEfect(int index)
    {
        /*if(Camera.main != null)
        {
            Camera camara = Camera.main;
            ActiveEfectIndex = index;
            for (int i = 0; i < camara.transform.childCount; i++)
            {
                camara.transform.GetChild(i).gameObject.SetActive(i==ActiveEfectIndex);
            }
        }*/
        if(Camera.main != null)
        {
            Camera camara = Camera.main;
            if(camara.TryGetComponent(out ColorBlindFilter colorBlindFilter))
            {
                colorBlindFilter.mode = (ColorBlindMode)index;
            }
        }
    }

    //VolumeControl
    public void SetAudioVolume(float value, string ExposeValue = "Volume"){
        if(value<0 || value>500f){ Debug.LogWarning("El valor de volumen debe ser un float entre 0 y 1"); return;}
        if(audioMix!=null)
        {
            audioMix.SetFloat(ExposeValue, value==0?-80f:Mathf.Log10(value) * 20);
        }
    }
}
