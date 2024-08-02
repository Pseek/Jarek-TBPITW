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
        if (masterValue <= 0)
        {
            mixer.SetFloat("MasterVolume", 0);
        }
        else
        {
            mixer.SetFloat("MasterVolume", Mathf.Log(masterValue) * 20);   
        }
        PlayerPrefs.SetFloat("MasterVolume", masterValue);
    }
    public void SetLevelMusic(float musicValue)
    {
        if (musicValue <= 0)
        {
            mixer.SetFloat("MusicVolume", 0);
        }
        else
        {
            mixer.SetFloat("MusicVolume", Mathf.Log(musicValue) * 20);
        }
        PlayerPrefs.SetFloat("MusicVolume", musicValue);
    }
    public void SetLevelSFX(float sfxValue)
    {
        if (sfxValue <= 0)
        {
            mixer.SetFloat("SFXVolume", 0);
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log(sfxValue) * 20);
        }
        PlayerPrefs.SetFloat("SFXVolume", sfxValue);
    }

}
