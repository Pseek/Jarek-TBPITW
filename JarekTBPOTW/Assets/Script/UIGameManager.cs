using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIGameManager : MonoBehaviour
{
    public GM gM;
    public Sprite medailleDor;
    public Sprite medailleDargent;
    public Sprite medailleBronze;
    public Sprite medailleNull;
    public Image medaillePosition;

    public void Update()
    {
        if(gM.elapsedTime >= 120f)
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
