using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour {
    private float speed = 1.8f;
    private int movementDirection = 1;
    private Vector3 velocity;
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
    }

    // Update is called once per frame
    void Update() {
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

            // Cambiamos la direccion de la tortuga
            ChangeDirection();
        }

        if (other.gameObject.CompareTag("Player")) {
            velocity = Vector3.zero;
            
        }
    }


    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("[Turtle.OnCollisionEnter2D]");

        if(other.collider.gameObject.CompareTag("Turtle")) {
            Debug.Log("[Turtle.OnCollisionEnter2D] colision de " + gameObject.name);

            // Mientras gira la tortuga está parada
            velocity = Vector3.zero;
            animator.SetBool("turning", true);
        }
    }

    public void RestartMovement()
    {
        ChangeDirection();
        animator.SetBool("turning", false);
    }

    private void ChangeDirection()
    {
        movementDirection *= -1;

        // Change object direction
        velocity = Vector3.right * movementDirection * speed;

        // Change sprite direction
        Vector3 newScale = transform.localScale;
        newScale.x = -movementDirection;
        transform.localScale = newScale;
    }
}
