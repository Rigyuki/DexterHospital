using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioMgr : MonoBehaviour
{
    public static AudioMgr Instance { get; private set; }
    public AudioSource _effectAudio;
    public AudioSource _musicAudio;
    public AudioSource _dialogueAudio;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayEffect(string _soundName)
    {
        _effectAudio.PlayOneShot(Resources.Load<AudioClip>("Audio/“Ù–ß/" + _soundName));
    }
    public void PlayMusic(string _musicName, bool loop=true)
    {
        Debug.Log("BGM path = " + "Audio/BGM" + _musicName);
        _musicAudio.loop = loop;
        _musicAudio.clip = Resources.Load<AudioClip>("Audio/BGM/" + _musicName);
        
        _musicAudio.Play();
    }

    public void StopMusic()
    {
        _musicAudio.Stop();
    }


    //Ω«…´”Ô“Ù
    public void playDialogue(string _dialoguepath)
    {
        _dialogueAudio.Stop();
        _dialogueAudio.PlayOneShot(Resources.Load<AudioClip>("Audio/dialog" + _dialoguepath));
    }

    public void StopDialog()
    {
        _dialogueAudio.Stop();
    }
}