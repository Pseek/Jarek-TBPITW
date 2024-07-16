using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionRoom : MonoBehaviour
{
    public GameObject virtualCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);
        }
        if (collision.CompareTag("Bal") && !collision.isTrigger) 
        {
            Debug.Log("je suis actif");
            collision.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
        }
        if (collision.CompareTag("Bal") && !collision.isTrigger)
        {
            collision.gameObject.SetActive(false);
        }
    }
}
