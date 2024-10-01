using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class Timer : MonoBehaviour
{
    public GM gM;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerPause;
    void Awake()
    {
        Time.timeScale = 0f;
    }
    void Update()
    {
        gM.elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gM.elapsedTime / 60);
        int seconds = Mathf.FloorToInt(gM.elapsedTime % 60);
        int millieme =Mathf.FloorToInt((gM.elapsedTime - (int)gM.elapsedTime) *10f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:0}",minutes,seconds,millieme);
        timerPause.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
    }
}
