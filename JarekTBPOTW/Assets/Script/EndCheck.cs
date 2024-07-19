using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCheck : MonoBehaviour
{
    public GM gM;
    public PlayerMovement pM;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting && gM.nbrBAL_a_D == 7)
        {
            gM.isWin = true;
        }
    }
}
