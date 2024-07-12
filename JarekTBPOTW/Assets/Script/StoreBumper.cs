using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBumper : MonoBehaviour
{
    public Rigidbody2D rb2DPlayer;
    public float bumperForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb2DPlayer.velocity = new Vector2(rb2DPlayer.velocity.x, bumperForce);
        }
    }
}
