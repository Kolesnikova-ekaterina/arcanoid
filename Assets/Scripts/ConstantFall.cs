using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantFall : MonoBehaviour
{
    public float fallSpeed = -5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, fallSpeed);
    }
}
