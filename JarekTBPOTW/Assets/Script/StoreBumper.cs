using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBumper : MonoBehaviour
{
    public Rigidbody2D _rb2D;
    public PlayerMovement pM;
    public float bumperForce;
    public bool isStored;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            pM._canDash = true;
            isStored = true;
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, bumperForce);
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
