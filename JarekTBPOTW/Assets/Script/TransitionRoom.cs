using System;
using System.Collections;
using System.Collections.Generic;
using SimpleWaypointIndicators;
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
            gameObject.transform.Find("GrEnemyPoint").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
            gameObject.transform.Find("GrEnemyPoint").gameObject.SetActive(false);
        }
    }
}
