using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class SensibilityManager : MonoBehaviour, IManager
{
    public bool isActive;
    [Header("Vibration")]
    [Range(0.0f, 10.0f)] public float VibrationSensibility;

    [Header("Color")]
    public bool foo1;
    [Header("Volume")]
    [SerializeField] AudioMixer audioMix;

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

    public void CrearControlHUD(){
        //Init Resources
        audioMix = Resources.Load<AudioMixer>("Audios/GeneralAudioMix");

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
        _vibrationControl.transform.GetChild(0).GetChild(0).GetComponent<Slider>().onValueChanged.RemoveAllListeners();
        _vibrationControl.transform.GetChild(0).GetChild(0).GetComponent<Slider>().onValueChanged.AddListener((float sliderValue)=>{SetVibrationPower(sliderValue);});
    }
    void ColorConfig(){
        //
    }

    void VolumeConfig(){
         _volumeControl.transform.GetChild(0).GetChild(0).GetComponent<Slider>().onValueChanged.RemoveAllListeners();
        _volumeControl.transform.GetChild(0).GetChild(0).GetComponent<Slider>().onValueChanged.AddListener((float sliderValue)=>{SetAudioVolume(sliderValue);});
    }

    //VibrationControl
    public void SetVibrationPower(float value){
        Managers manager = GetComponentInParent<Managers>();
        SerialReader serialReader = (SerialReader)manager._managers[1];
        serialReader.SendSerialPortData(SerialReader.CerebroComds.ConfigVibration+"["+value+"]");
    }

    //ColorControl

    //VolumeControl
    public void SetAudioVolume(float value){
        if(value<0 || value>500f){ Debug.LogWarning("El valor de volumen debe ser un float entre 0 y 1"); return;}
        if(audioMix!=null)
        {
            audioMix.SetFloat("Volume", value==0?-80f:Mathf.Log10(value) * 20);
        }
    }
}
