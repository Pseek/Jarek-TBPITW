using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitDog : MonoBehaviour
{
    public bool isLimit = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            isLimit = false;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            isLimit = true;
        }
    }
}
