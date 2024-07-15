using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dumpster : MonoBehaviour
{
    public float CdDumpster;
    public Sprite fullDumpster;
    public Sprite emptyDumpster;
    public SpriteRenderer spriteRenderer;

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
        spriteRenderer.sprite = emptyDumpster;
        yield return new WaitForSeconds(CdDumpster);
        spriteRenderer.sprite = fullDumpster;  
        gameObject.GetComponent <BoxCollider2D>().enabled = true;
        StopCoroutine(CDFallCooldown());
    }
}
