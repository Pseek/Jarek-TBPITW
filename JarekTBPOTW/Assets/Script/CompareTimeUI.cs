using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CompareTimeUI : MonoBehaviour
{
    public GM gM;
    public TextMeshProUGUI timerCurrentTime;
    public TextMeshProUGUI timercCompareTime;

    public void CompareTimer()
    {
        float timerCompare = gM.elapsedTime - gM.rD.listTimerBAL[gM.nbrBAL_a_D - 1];
        int minutes = Mathf.FloorToInt(timerCompare / 60);
        int seconds = Mathf.FloorToInt(timerCompare % 60);
        int millieme = Mathf.FloorToInt((timerCompare - (int)timerCompare) * 10f);
        if (timerCompare >= 0)
        {
            timercCompareTime.color = Color.green;
            timercCompareTime.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
        }else if (timerCompare < 0)
        {
            timercCompareTime.color = Color.red;
            timercCompareTime.text = string.Format("-{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
        }

        minutes = Mathf.FloorToInt(gM.elapsedTime / 60);
        seconds = Mathf.FloorToInt(gM.elapsedTime % 60);
        millieme = Mathf.FloorToInt((gM.elapsedTime - (int)gM.elapsedTime) * 10f);
        timerCurrentTime.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
    }
}
