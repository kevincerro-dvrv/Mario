using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    private float speed = 3f;
    private float jumpForce = 6.5f;
    private Vector3 velocity;
    private int movementDirection;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && movementDirection != 1) {
            Walk(-1);
        } else if (Input.GetKey(KeyCode.RightArrow) && movementDirection != -1) {
            Walk(1);
        } else {
            Walk(0);
        }

        Jump();
    }

    private void Walk(int movementDirection)
    {
        velocity = Vector3.right * movementDirection * speed;
        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
