using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CompareTimeUI : MonoBehaviour
{
    public GM gM;
    public GameObject tCUI;
    public TextMeshProUGUI timerCurrentTime;
    public TextMeshProUGUI timercCompareTime;
    public float chrono;
    private bool isAffiched = false;

    public Sprite medailleDor;
    public Sprite medailleDargent;
    public Sprite medailleBronze;
    public Sprite medailleNull;
    public Image medaillePosition;

    public void Update()
    {
        chrono += Time.deltaTime;
        if (gM.isCheckpoint)
        {
            chrono = 0f;
            CompareTimer();
            MedailleOnCheckpoint();
            gM.isCheckpoint = false;
            isAffiched = true;
            tCUI.SetActive(true);   
        }
        if (chrono > 3f && !gM.isCheckpoint && isAffiched)
        {
            tCUI.SetActive(false);
            chrono = 0f;
            isAffiched = false;
        }


    }   
    public void CompareTimer()
    {
        float timerCompare = gM.rD.listTimerBALData[gM.nbrBAL_a_D - 1] - gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1];
        
        if(timerCompare < 0f) 
        {
            timerCompare = timerCompare * -1f;
        }
        int minutes = Mathf.FloorToInt(timerCompare / 60);
        int seconds = Mathf.FloorToInt(timerCompare % 60);
        int millieme = Mathf.FloorToInt((timerCompare - (int)timerCompare) * 10f);

        if(gM.rD.listTimerBALData[gM.nbrBAL_a_D - 1] == 0)
        { 
            timercCompareTime.color = Color.white;
            timercCompareTime.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
            gM.rD.listTimerBALData.RemoveAt(gM.nbrBAL_a_D - 1);
            gM.rD.listTimerBALData.Insert(gM.nbrBAL_a_D - 1, gM.elapsedTime);
        }
        else if (gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1] < gM.rD.listTimerBALData[gM.nbrBAL_a_D - 1])
        {
            timercCompareTime.color = Color.green;
            timercCompareTime.text = string.Format("-{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
        }
        else if (gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1] > gM.rD.listTimerBALData[gM.nbrBAL_a_D - 1])
        {
            timercCompareTime.color = Color.red;
            timercCompareTime.text = string.Format("+{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);
        }

        minutes = Mathf.FloorToInt(gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1] / 60);
        seconds = Mathf.FloorToInt(gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1] % 60);
        millieme = Mathf.FloorToInt((gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1] - (int)gM.rC.listTimerBALCurrent[gM.nbrBAL_a_D - 1]) * 10f);
        timerCurrentTime.text = string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, millieme);

        if (gM.rD.listTimerBALData[gM.nbrBAL_a_D - 1] > gM.elapsedTime && gM.isCheckpoint)
        {
            gM.rD.listTimerBALData.RemoveAt(gM.nbrBAL_a_D - 1);
            gM.rD.listTimerBALData.Insert(gM.nbrBAL_a_D - 1, gM.elapsedTime);
        }
        gM.Save();
    }

    private void MedailleOnCheckpoint()
    {
        if (gM.elapsedTime >= 120f)
        {
            medaillePosition.sprite = medailleNull;
        }
        else if (gM.elapsedTime >= 90f)
        {
            medaillePosition.sprite = medailleBronze;
        }
        else if (gM.elapsedTime >= 60f)
        {
            medaillePosition.sprite = medailleDargent;
        }
        else if (gM.elapsedTime < 60f)
        {
            medaillePosition.sprite = medailleDor;
        }
    }
}
