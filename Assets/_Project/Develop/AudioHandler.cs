using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHandler
{
    private float DeadZone = 0.01f;
    
    private const float OffVolumeValue = -80;
    private const float OnVolumeValue = 0;
    
    private const string MusicKey = "MusicVolume";
    private const string SoundKey = "SoundsVolume";
    
    private AudioMixer _audioMixer;

    public AudioHandler(AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;
    }
    
    public bool IsMusicOn() => IsVolumeOn(MusicKey);
    public bool IsSoundOn() => IsVolumeOn(SoundKey);
    
    public void OnMusicVolume() => OnVolume(MusicKey);
    public void OffMusicVolume() => OffVolume(MusicKey);
    
    public void OnSoundVolume() => OnVolume(SoundKey);
    public void OffSoundVolume() => OffVolume(SoundKey);
    
    private bool IsVolumeOn(string key) 
        => _audioMixer.GetFloat(key, out float volume) && Mathf.Abs(volume - OnVolumeValue) <= DeadZone;
    
    private void OnVolume(string key) => _audioMixer.SetFloat(key,OnVolumeValue);
    private void OffVolume(string key) => _audioMixer.SetFloat(key,OffVolumeValue);
}
