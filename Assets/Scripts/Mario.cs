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
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded()) {
            if (Input.GetKey(KeyCode.LeftArrow) && movementDirection != 1) {
                Walk(-1);
            } else if (Input.GetKey(KeyCode.RightArrow) && movementDirection != -1) {
                Walk(1);
            } else {
                Walk(0);
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        transform.position = transform.position + velocity * Time.deltaTime;
    }

    private void Walk(int movementDirection)
    {
        velocity = Vector3.right * movementDirection * speed;

        if (movementDirection == 1 || movementDirection == -1) {
            animator.SetBool("walking", true); 

            // Set animation direction
            Vector3 marioScale = transform.localScale;
            marioScale.x = movementDirection * -1;
            transform.localScale = marioScale;
        } else {
            animator.SetBool("walking", false); 
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y -= 0.65f;

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.05f);
        if (hit.collider != null) {
            animator.SetBool("jumping", false);

            return true;
        }

        animator.SetBool("jumping", true);

        return false;
    }
}
