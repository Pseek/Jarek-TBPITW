using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBumper : MonoBehaviour
{ 
    public PlayerMovement pM;
    public float bumperForce;
    public bool isStored;
    public AudioSource aS;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            pM._canDash = true;
            isStored = true;
            collision.gameObject.GetComponent<PlayerMovement>().sB = this;
            aS.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isStored = false;
        }
    }
}
