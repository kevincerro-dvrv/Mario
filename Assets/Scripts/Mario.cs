using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Mario : MonoBehaviour {
    private const float INMUTY_TIME = 5f;

    private float speed = 3f;
    private float jumpForce = 6.5f;
    private Vector3 velocity;
    private float brakeAcceleration = 7.0f;
    private int movementDirection;

    private Rigidbody2D rb;
    private Animator animator;

    //Esta variable se pone a true al inicio del salto para protegerlo de la detección del suelo
    private bool takingOff;

    //Variable que controla si Mario está en la animación de morir, en cuyo caso no se responde a las ordenes del jugador
    private bool dying = false;

    //Variable que concede inmunidad a Mario tras su espaneo
    private bool inmune = false;

    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();        
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(FireBlink());
    }

    // Update is called once per frame
    void Update() {
        if(dying) {
            if(transform.position.y < -10) {
                Destroy(gameObject);
            }
            return;
        }

        if(IsGrounded()) {
            if(Input.GetKey(KeyCode.LeftArrow) && movementDirection != 1) {
                Walk(-1);
            } else if(Input.GetKey(KeyCode.RightArrow) && movementDirection != -1) {
                Walk(1);
            } else {
                //Si mario se está moviendo restamos velocidad usando brakingAceleration
                if(Mathf.Abs(velocity.x) > 0.05f) {
                    velocity.x -= movementDirection * brakeAcceleration * Time.deltaTime;
                    if( ! animator.GetBool("braking")) {
                        animator.SetBool("braking", true);
                    }
                } else {
                    //Si mario se mueve muy lento, le ponemos la velocidad a cero para que se quede parado definitivamente
                    Walk(0);
                }
            }

            if(Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            } else if(! takingOff) {
                animator.SetBool("jumping", false);
            }
        }

        transform.position = transform.position + velocity * Time.deltaTime;
        
    }


    private void Walk(int direction) {
        movementDirection = direction;
        velocity = Vector3.right * movementDirection * speed;

        if(movementDirection == 1 || movementDirection == -1) {
            animator.SetBool("walking", true);
            if(animator.GetBool("braking")) {
                animator.SetBool("braking", false);
            }
            Vector3 marioScale = transform.localScale;
            marioScale.x = movementDirection * -1;
            transform.localScale = marioScale;
        } else {
            animator.SetBool("walking", false);
            animator.SetBool("braking", false);
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        takingOff = true;
        Invoke("ResetTakingOff", 0.05f);
        animator.SetBool("jumping", true);
    }

    private bool IsGrounded() {
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y -= 0.65f;

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.05f);
        if(hit.collider != null) {
            return true;
        }


        //Si no se encuentra suelo, devuelve false
        return false;
    }

    private void ResetTakingOff() {
        takingOff = false;
    }

    public void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Turtle")) {
            Turtle turtle = other.gameObject.GetComponent<Turtle>();
            if(turtle.TurtleIsActive()) {
                if( ! inmune) {
                    velocity = Vector3.zero;
                    dying = true;
                    animator.SetBool("shocking", true);
                    //Ponemos a Mario en la capa NoCollisions
                    gameObject.layer = LayerMask.NameToLayer("NoCollisions");
                    rb.gravityScale = 0f;

                    GameManager.instance.MarioDied();

                    Invoke("FallingOver", 0.3f);
                }
            } else {
                turtle.GetOffScene(transform.position);
            }
        }
    }

    public void FallingOver() {
        animator.SetBool("falling_over", true);
        rb.gravityScale = 1f;
    }


    //Método llamado tras espanear a Mario para inicializar lo que sea necesario
    public void SpawnInicialization() {
        inmune = true;
        Invoke("EndInmunity", INMUTY_TIME);
        blinkCoroutine = StartCoroutine(Blink());
    }

    private void EndInmunity() {
        inmune = false;
        StopCoroutine(blinkCoroutine);
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

    }

    private IEnumerator Blink() {
        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        while(true) {
            Color color = spriteRenderer.color;
            color.a = Util.MarioBlink(Time.time);
            spriteRenderer.color = color;
            yield return null;
        }
    }

    private IEnumerator FireBlink() {
        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        while(true) {
            Color color = spriteRenderer.color;
            
            if (Util.MarioFireBlink(Time.time) == 1) {
                spriteRenderer.color = Color.white;
            } else {
                spriteRenderer.color = Color.red;
            }

            yield return null;
        }
    }
}

