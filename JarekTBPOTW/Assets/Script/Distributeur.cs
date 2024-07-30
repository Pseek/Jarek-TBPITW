using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Distributeur : MonoBehaviour
{
    public PlayerMovement pM;
    public TrailRenderer tr;
    public GameObject buttonInterract;
    public bool isDrinked;

    private void Start()
    {
        isDrinked = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pM._isInterracting)
        {
            collision.GetComponent<PlayerMovement>().GetBuffSpeed();
            StartCoroutine(CdDistributeur());
            buttonInterract.SetActive(false);
            isDrinked=false;
        }
        if (collision.CompareTag("Player") && isDrinked == true)
        {
            buttonInterract.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isDrinked == true)
        {
            buttonInterract.SetActive(false);
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
