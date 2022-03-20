using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundsSource;

    [SerializeField] private AudioClip[] _soundClips;
    [SerializeField] private AudioClip[] _musicClips;
    public enum soundList
    {
        CorrectChoice = 0,
        WrongChoice = 1,
        DoorOpen = 2,
        DoorClose = 3,
        Newspaper = 4,
        PlayCard = 5,
        ShuffleCards = 6,
        CashRegister = 7

    }

    public enum musicList
    {
        Music1 = 0,
        Music2 = 1,
        WinMusic = 2,
        LoseMusic = 3,
        MainMenuMusic = 4

    }

    #region Singleton

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public void PlaySound(soundList audio)
    {
        var i = (int)audio;
        _soundsSource.PlayOneShot(_soundClips[i]);
    }

    public void ChangeMusic(int music)
    {
        _musicSource.clip = _musicClips[music];
        _musicSource.Play();
    }

    public void MusicStop()
    {
        _musicSource.Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        _musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
