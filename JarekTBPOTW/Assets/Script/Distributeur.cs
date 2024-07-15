using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Distributeur : MonoBehaviour
{
    public PlayerMovement pM;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting)
        {
            collision.GetComponent<PlayerMovement>().GetBuffSpeed();
            StartCoroutine(CdDistributeur());
        }
    }
    IEnumerator CdDistributeur()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(20f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        StopCoroutine(CdDistributeur());
    }
    
}
