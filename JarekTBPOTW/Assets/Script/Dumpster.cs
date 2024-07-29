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
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            spriteRenderer.sprite = emptyDumpster;
            
        }
    }
}