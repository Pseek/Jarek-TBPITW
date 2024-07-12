using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumpster : MonoBehaviour
{
    public PlayerMovement pM;
    public float CdDumpster;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().StartFallDumpster();
            StartCoroutine(CDFallCooldown());
        }
    }

    IEnumerator CDFallCooldown()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(CdDumpster);
        gameObject.GetComponent <BoxCollider2D>().enabled = true;
        StopCoroutine(CDFallCooldown());
    }
}
