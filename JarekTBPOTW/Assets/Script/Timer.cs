using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Internal;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerFinalText;
    public float elapsedTime;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int millieme =Mathf.FloorToInt((elapsedTime - (int)elapsedTime) *10f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:0}",minutes,seconds,millieme);
        timerFinalText.text = timerText.text;
    }
}
