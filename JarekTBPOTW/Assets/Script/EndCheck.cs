using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheck : MonoBehaviour
{
    public GM gM;
    public PlayerMovement pM;
    public GameObject EndIndication;
    public GameObject ArrowIndication;
    

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting && gM.nbrBAL_a_D > 19)
        {
            gM.ChangesScene("UIWinMenu");
            gM.tM.AddTimeList(gM.elapsedTime);
            gM.tM.SortList();
            gM.Save();
        }
    }

    public void Update()
    {
        if (gM.nbrBAL_a_D > 19)
        {
            EndIndication.SetActive(true);
            ArrowIndication.SetActive(true);    
        }
    }
}
