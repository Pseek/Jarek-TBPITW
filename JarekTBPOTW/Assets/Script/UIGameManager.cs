using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIGameManager : MonoBehaviour
{
    public GM gM;
    public UnityEvent CheckWin;
    public Timer timer;
    public Sprite medailleDor;
    public Sprite medailleDargent;
    public Sprite medailleBronze;
    public Sprite medailleNull;
    public Image medaillePosition;


    public void Update()
    {
        if (gM.isWin)
        {
            CheckWin.Invoke();
        }
        if(timer.elapsedTime >= 60f)
        {
            medaillePosition.sprite = medailleNull;
        }
        else if (timer.elapsedTime >= 45f)
        {
            medaillePosition.sprite = medailleBronze;
        }
        else if (timer.elapsedTime >= 30f)
        {
            medaillePosition.sprite = medailleDargent;
        }
        else if (timer.elapsedTime < 30f)
        {
            medaillePosition.sprite = medailleDor;
        }               
    }
}
