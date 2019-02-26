using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D rb;

    private bool facingLeft = true;

    public bool isGrounded;

    public Transform groundCheck;
    public LayerMask whatIsGround;

    private int maxJumps = 3;
    public int currentJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        //Debug.Log("Player Hit" + hit.gameObject.name);
    }

    void Update()
    {
        if (isGrounded == true)
        {
            currentJumps = maxJumps;
        }

        if(Input.GetKeyDown("space") && currentJumps >= 1)
        {
            rb.velocity = Vector2.up * jumpForce;
            currentJumps--;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (facingLeft == true && moveInput > 0)
        {
            Flip();
        }
        else if (facingLeft == false && moveInput < 0)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        Vector2 floorDirection = new Vector2(0,-1);
        Debug.DrawRay(groundCheck.position, floorDirection * 0.2f, Color.red);
        isGrounded = Physics2D.Raycast(groundCheck.position, floorDirection, 0.2f, whatIsGround);

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
