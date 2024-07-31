using System;
using System.Collections;
using System.Collections.Generic;
using SimpleWaypointIndicators;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionRoom : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject musicBox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(true);
            musicBox.SetActive(true);
            gameObject.transform.Find("GrEnemyPoint").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            virtualCam.SetActive(false);
            musicBox.SetActive(false);
            gameObject.transform.Find("GrEnemyPoint").gameObject.SetActive(false);
        }
    }
}
