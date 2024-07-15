using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBumper : MonoBehaviour
{
    public Rigidbody2D _rb2D;
    public float bumperForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, bumperForce);
        }
    }
}
