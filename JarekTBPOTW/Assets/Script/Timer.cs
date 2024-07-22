using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerFinalText;
    public TextMeshProUGUI timerSubmit;
    public TextMeshProUGUI highScore;
    public GM gM;
    public float currentHighScore;

    void Start()
    {
        highScore.text = PlayerPrefs.GetFloat("HighTime",0f).ToString();
    }
        
    void Update()
    {
        gM.elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gM.elapsedTime / 60);
        int seconds = Mathf.FloorToInt(gM.elapsedTime % 60);
        int millieme =Mathf.FloorToInt((gM.elapsedTime - (int)gM.elapsedTime) *10f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:0}",minutes,seconds,millieme);
        timerFinalText.text = timerText.text;
    }

    public void HighScore()
    {
        if (gM.elapsedTime < PlayerPrefs.GetFloat("HighTime",currentHighScore))
        {
            PlayerPrefs.SetFloat("HighTime", gM.elapsedTime);
            highScore.text = gM.elapsedTime.ToString();
            currentHighScore = gM.elapsedTime;
        }
    }
}
