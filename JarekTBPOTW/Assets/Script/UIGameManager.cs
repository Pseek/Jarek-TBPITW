using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIGameManager : MonoBehaviour
{
    public GM gM;
    public UnityEvent CheckWin;
    public Sprite medailleDor;
    public Sprite medailleDargent;
    public Sprite medailleBronze;
    public Sprite medailleNull;
    public Image medaillePosition;
    public Timer timer;

    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void Update()
    {
        if (gM.isWin)
        {
            CheckWin.Invoke();
            timer.HighScore();
        }
        if(gM.elapsedTime >= 60f)
        {
            medaillePosition.sprite = medailleNull;
        }
        else if (gM.elapsedTime >= 45f)
        {
            medaillePosition.sprite = medailleBronze;
        }
        else if (gM.elapsedTime >= 30f)
        {
            medaillePosition.sprite = medailleDargent;
        }
        else if (gM.elapsedTime < 30f)
        {
            medaillePosition.sprite = medailleDor;
        }               
    }
}
