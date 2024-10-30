using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ExtraBlueBlockScript : BlockScript
{
    public float speed = 0.5f;
    public Vector2 direction;
    public Vector2 pos1, pos2;
    private Rigidbody2D rb;
    private bool topos2 = true;
    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        
    }
    private bool onthepose()
    {
        var xMax = Camera.main.orthographicSize * Camera.main.aspect;
        return (math.sqrt(math.pow(transform.position.x - pos1.x,2) + math.pow(transform.position.y - pos1.y, 2)) < 0.5f && !topos2) || 
            (math.sqrt(math.pow(transform.position.x - pos2.x, 2) + math.pow(transform.position.y - pos2.y, 2)) < 0.5f && topos2) ||
            (transform.position.x < -xMax + 0.5f && direction.x <= 0 ||
            transform.position.x > xMax - 0.5f && direction.x >= 0);
    }
    // Update is called once per frame
    protected override void Update()
    {
        Vector2 velocity = rb.velocity;
        if (onthepose())
        {
           // print("HA");
            topos2 = !topos2;
            direction.x *= -1.0f;
            direction.y *= -1.0f;
        }
        velocity = direction;
        rb.velocity = velocity;
       // print($"{velocity}" );
    }
}
