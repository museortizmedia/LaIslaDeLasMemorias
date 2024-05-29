using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "new Sfx", menuName = "AudioEvent/new Audio")]
public class Audio : ScriptableObject
{
    [System.Serializable]
    public struct AudioParametersStruct{
        public string AudioName;
        public AudioClip[] AudioClips;
        public float Volume;
        public float Pitch;
        public bool Loop;
        public float StartDelay;
        public AudioMixerGroup Canal;
    }

    public AudioParametersStruct AudioParameters;
    private AudioSource _audioSurce;
    GameObject _sourceGO;
    public void PlayAudio(){
        if(_sourceGO==null) { BuscarSource(); }
        
        _audioSurce.clip = AudioParameters.AudioClips[Random.Range(0, AudioParameters.AudioClips.Length)];
        _audioSurce.volume = AudioParameters.Volume;
        _audioSurce.pitch = AudioParameters.Pitch;
        _audioSurce.loop = AudioParameters.Loop;
        _audioSurce.PlayDelayed(AudioParameters.StartDelay);
    }
    void BuscarSource(){
        GameObject existGO = GameObject.Find($"Audio {AudioParameters.AudioName}");
        if(existGO==null)
        {
            _sourceGO = new GameObject($"Audio {AudioParameters.AudioName}");
            _audioSurce = _sourceGO.AddComponent<AudioSource>();
            _audioSurce.outputAudioMixerGroup = AudioParameters.Canal;
        }else{
            _sourceGO = existGO;
        }        
    }
    public void StopAudio()
    {
        if(_audioSurce!= null && _audioSurce.isPlaying)
        {
            _audioSurce.Stop();
        }
    }
    public void ChangeVolume(float value)
    {
        if(_audioSurce.isPlaying)
        {
            _audioSurce.volume = value;
        }
    }
}
