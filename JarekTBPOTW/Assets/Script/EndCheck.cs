using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheck : MonoBehaviour
{
    public GM gM;
    public PlayerMovement pM;
    public GameObject EndIndication;
    public GameObject ArrowIndication;
    public int nbrBalEnd;
    public int nbrBalRestant;
  
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting && gM.nbrBAL_a_D > nbrBalEnd)
        {
            gM.ChangesScene("UIWinMenu");
            gM.tM.AddTimeList(gM.elapsedTime);
            gM.tM.SortList();
            gM.Save();
        }
    }

    public void Update()
    {
        gM.nbrBALEndUI = nbrBalEnd + nbrBalRestant;
        if (gM.nbrBAL_a_D > nbrBalEnd)
        { 
            EndIndication.SetActive(true);
            ArrowIndication.SetActive(true);
            gM.isWin = true;        
        }
        if (gM.isWin && gM.isStop)
        {
            gM.isWin = false;
        }
    }
}
