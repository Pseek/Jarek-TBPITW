using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTriggerDog : MonoBehaviour
{
    public bool isOnPointDog = false;
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            isOnPointDog = true;   
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            isOnPointDog = false;
        }
    }
}
