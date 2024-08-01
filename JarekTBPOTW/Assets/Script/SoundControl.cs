using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider master;
    public Slider music;
    public Slider sfx;
    void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        music.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    public void SetLevelMaster (float masterValue)
    {
        mixer.SetFloat("MasterVolume" , Mathf.Log(masterValue) * 20 );
        PlayerPrefs.SetFloat("MasterVolume" , masterValue );
    }
    public void SetLevelMusic(float musicValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log(musicValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicValue);
    }
    public void SetLevelSFX(float sfxValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log(sfxValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxValue);
    }

}
