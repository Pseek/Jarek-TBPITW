using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPauseManager : MonoBehaviour
{
    public GM gM;
    public GameObject menuPause;
    public bool ShowMenuPause = false;

    public void Update()
    {
        if (gM.isPause == true && ShowMenuPause == true)
        {
            gM.TimeScaleOne();
            ShowMenuPause = false;
            gM.isPause = false;
            menuPause.SetActive(false);
        }
        if (gM.isPause == true)
        {
            gM.TimeScaleZero();
            gM.isPause = false;
            ShowMenuPause = true;
            menuPause.SetActive(true);
        }
    }
}
