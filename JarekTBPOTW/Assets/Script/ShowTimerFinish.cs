using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTimerFinish : MonoBehaviour
{
    public GM gM;
    public TextMeshProUGUI finishTimer;
    public TextMeshProUGUI highScore;

    public void Update()
    {
        float time = gM.tM.list[0];
        int minutes = Mathf.FloorToInt(gM.elapsedTime / 60);
        int seconds = Mathf.FloorToInt(gM.elapsedTime % 60);
        int millieme = Mathf.FloorToInt((gM.elapsedTime - (int)gM.elapsedTime) * 10f);
        finishTimer.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);

        minutes = Mathf.FloorToInt(time / 60);
        seconds = Mathf.FloorToInt(time % 60);
        millieme = Mathf.FloorToInt((time - (int)time) * 10f);
        highScore.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
    }
}