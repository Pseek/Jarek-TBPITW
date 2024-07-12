using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    public PlayerMovement pM;
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false ;
        }
    }
}
