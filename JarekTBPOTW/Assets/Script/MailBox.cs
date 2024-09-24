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
    public GameObject buttonBal;
    public AudioSource aS;
    public bool isBAL;

    private void Start()
    {
        isBAL = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting)
        {
            gM.isCheckpoint = true;
            isBAL = false;
            anim.SetBool("BalAnim", true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            pointerBAL.SetActive(false);
            upPointerBal.SetActive(false);
            buttonBal.SetActive(false);
            aS.Play();
            gM.AddBAL(1);
        }

        if (collision.CompareTag("Player") && isBAL == true)
        {
            upPointerBal.SetActive(false);
            buttonBal.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isBAL == true)
        {
            buttonBal.SetActive(false);
            upPointerBal.SetActive(true);
        }
    }
}
