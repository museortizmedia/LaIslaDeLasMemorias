using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sfx", menuName = "ScriptableObject/CreateNewSfxAsset")]
public class AudioSfx : ScriptableObject
{
    [System.Serializable]
    public struct AudioParametersStruct{
        public string AudioName;
        public AudioClip[] AudioClips;
        public float Volume;
        public float Pitch;
        public bool Loop;
        public float StartDelay;
    }

    public AudioParametersStruct AudioParameters;
    private AudioSource _audioSurce;
    GameObject _sourceGO;
    public void PlayAudio(){
        if(_sourceGO==null){ _sourceGO = new GameObject($"Audio {AudioParameters.AudioName}"); _audioSurce = _sourceGO.AddComponent<AudioSource>(); }
        
        _audioSurce.clip = AudioParameters.AudioClips[Random.Range(0, AudioParameters.AudioClips.Length)];
        _audioSurce.volume = AudioParameters.Volume;
        _audioSurce.pitch = AudioParameters.Pitch;
        _audioSurce.loop = AudioParameters.Loop;
        _audioSurce.PlayDelayed(AudioParameters.StartDelay);
    }
    public void StopAudio(){if(_audioSurce.isPlaying){_audioSurce.Stop();}}
}
