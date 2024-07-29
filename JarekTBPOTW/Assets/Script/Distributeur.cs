using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Distributeur : MonoBehaviour
{
    public PlayerMovement pM;
    public TrailRenderer tr;
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
        tr.emitting = true;
        yield return new WaitForSeconds(20f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        tr.emitting = false;
        StopCoroutine(CdDistributeur());
    }
    
}
