using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement pM;
    public GM gM;
    public GameObject pointerBAL;
    public GameObject upPointerBal;
    
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting)
        {
            gM.AddBAL(1);
            anim.SetBool("BalAnim", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            pointerBAL.SetActive(false);
            upPointerBal.SetActive(false);
        }
    }

    public void ResetLevel()
    {
        if(gM.resetLevel)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            pointerBAL.SetActive(true);
            upPointerBal.SetActive(true);
        }
    }
}
