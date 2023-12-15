using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour {
    private float speed = 1.8f;
    private float impulseToFall = 5f;
    private int movementDirection = 1;
    private Vector3 velocity;

    private Rigidbody2D rb;

    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        if(transform.position.x > 0) {
            movementDirection = -1;
        }
        velocity = Vector3.right * movementDirection *speed;
        Vector3 newScale = transform.localScale;
        newScale.x *= -movementDirection;
        transform.localScale = newScale;

        animator = GetComponent<Animator>();
        if(animator == null) {
            Debug.Log("[Turtle.Start] animator no encontrado");
        }

        rb = GetComponent<Rigidbody2D>();
        if(rb == null) {
            Debug.Log("[Turtle.Start] rb no encontrado");
        }
    }

    // Update is called once per frame
    void Update() {
        if(! IsGrounded()) {
        Debug.Log("[Turtle] " + gameObject.name + " En el aire");
        }

        if(transform.position.y < -10) {
            Destroy(gameObject);
        }

        Vector3 newPosition = transform.position;
        newPosition += velocity * Time.deltaTime;
        transform.position = newPosition;
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Floor1PlatformEdge")) {
            Vector3 newPosition = transform.position;
            newPosition.x *= -1;
            transform.position = newPosition;
        }

        if(other.gameObject.CompareTag("PipeEdge")) {
            //Si detectamos el PipeEdge, tenemos que teletrasportarnos al punto de espaneo
            if(transform.position.x > 0) {
                //Si estamos a la derecha nos vamos al punto de espaneo derecho
                transform.position = GameManager.instance.rightTurtleSpawnPoint.position;
            } else {
                transform.position = GameManager.instance.leftTurtleSpawnPoint.position;
            }
        }

        if(other.gameObject.CompareTag("Player") && IsGrounded()) {
            velocity = Vector3.zero;
            rb.AddForce((Vector3.up + Vector3.right * (transform.position.x - other.transform.position.x)) * 2.5f, ForceMode2D.Impulse);
            animator.SetBool("rotating", ! animator.GetBool("rotating"));
            if(TurtleIsActive()) {
                movementDirection = TurtleSign(transform.position.x - other.transform.position.x);
                //Por si Mario y la tortuga están perfectamente alineados
                if(movementDirection == 0) {
                    movementDirection = TurtleSign(transform.localScale.x) * -1;
                }
                velocity = Vector3.right * movementDirection *speed;
                Vector3 newScale = transform.localScale;
                newScale.x = -movementDirection;
                transform.localScale = newScale;
            }
        }
    }

    public bool TurtleIsActive() {
       return ! animator.GetBool("rotating");
    }

    public void GetOffScene(Vector3 marioPosition) {
        gameObject.layer = LayerMask.NameToLayer("NoCollisions");
        animator.SetBool("falling_and_spinning", true);
        rb.AddForce(Vector3.right * impulseToFall * TurtleSign(transform.position.x - marioPosition.x), ForceMode2D.Impulse);
    }

    private int TurtleSign(float f) {
        if(f<0) {
            return -1;
        } else if(f > 0) {
            return 1;
        }
        return 0;
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("[Turtle.OnCollisionEnter2D]");

        if(other.collider.gameObject.CompareTag("Turtle")) {
            Debug.Log("[Turtle.OnCollisionEnter2D] colision de " + gameObject.name);

            //Mientras gira la tortuga está parada
            velocity = Vector3.zero;
            animator.SetBool("turning", true);

        }
    }

    public void RestartMovement() {
        movementDirection *= -1;       
        velocity = Vector3.right * movementDirection * speed;
        Vector3 newScale = transform.localScale;
        newScale.x = -movementDirection;
        transform.localScale = newScale;
        animator.SetBool("turning", false);
    }

    private bool IsGrounded() {
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y -= 0.3f;

        LayerMask platformLayer = LayerMask.GetMask("Platform");

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.05f, platformLayer);
        if(hit.collider != null) {
            return true;
        }
        //Si no se encuentra suelo, devuelve false
        return false;
    }
}
