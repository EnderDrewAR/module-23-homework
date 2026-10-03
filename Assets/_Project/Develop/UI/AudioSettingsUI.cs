using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _soundsButton;
    
    private AudioHandler _audioHandler;

    public void Initialize(AudioHandler audioHandler)
    {
        _audioHandler = audioHandler;
        
        _musicButton.onClick.AddListener(ToggleMusic);
        _soundsButton.onClick.AddListener(ToggleSounds);
    }

    private void ToggleMusic()
    {
        if (_audioHandler.IsMusicOn())
            _audioHandler.OffMusicVolume();
        else
            _audioHandler.OnMusicVolume();
    }
    
    private void ToggleSounds()
    {
        if (_audioHandler.IsSoundOn())
            _audioHandler.OffSoundVolume();
        else
            _audioHandler.OnSoundVolume();
    }
    
    private void OnDestroy()
    {
        if (_musicButton != null)
            _musicButton.onClick.RemoveListener(ToggleMusic);

        if (_soundsButton != null)
            _soundsButton.onClick.RemoveListener(ToggleSounds);
    }
}
