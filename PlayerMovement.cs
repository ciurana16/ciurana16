using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //=================================================================
    //                        VARIABLE DECLARATION
    //=================================================================

    private float velocidad = 7; //Velocidad del jugador
    bool isJumping = false; //Para comprobar si está saltando
    bool isFalling = false; // Para comprobar si está cayendo
    bool isGrounded = true;
    private float potenciaSalto = 400; //Potencia de salto del jugador

    private Animator animator; //Para capturar el componente Animator del Jugador

    Rigidbody2D rb2d;
    SpriteRenderer spRd;


    //=================================================================
    //                         FUNCTION START
    //=================================================================

    void Start () {

        //2. Capturo y asocio los componentes Rigidbody2D y Sprite Renderer del Jugador
        rb2d = GetComponent<Rigidbody2D>();
        spRd = GetComponent<SpriteRenderer>();

        //Capturo y asocio el componente Animator del Jugador
        animator = GetComponent<Animator>();

    }

    //=================================================================
    //                      FUNCTION FIXED UPDATE             
    //=================================================================
	
	void FixedUpdate () {

        //========================
        //Horitzontal movement
        //========================

        float movimientoH = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = new Vector2(movimientoH * velocidad, rb2d.velocity.y);

        //========================
        //Flip Sprite on X axis 
        //========================

        if (movimientoH > 0)
        {
            spRd.flipX = false;
        }
        else if (movimientoH < 0)
        {
            spRd.flipX = true;
        }

        //========================
        //Jump
        //========================

        if (Input.GetButton("Jump") && !isJumping && !isFalling) //Si pulso la tecla de salto (espacio) y no estaba saltando
        {   
            rb2d.AddForce(Vector2.up * potenciaSalto);//Le aplico la fuerza de salto
            isJumping = true;//Digo que está saltando (para que no pueda volver a saltar)
            isGrounded = false;
            animator.SetBool("isJumping", true); //Si se está moviendo, reproduzco la animación
      
        }

        if (isJumping && rb2d.velocity.y < 0) // problema
            {
                isJumping = false;
                isFalling = true;
                isGrounded = false;
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
            }

        //========================
        //Running
        //========================

        if (movimientoH != 0)
        {
            animator.SetBool("isRunning", true); //Si se está moviendo, reproduzco la animación
        }
        else 
        {
            animator.SetBool("isRunning", false); //Si no, la paro
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Si el jugador colisiona con un objeto con la etiqueta suelo
        if (other.gameObject.CompareTag("Tilemap"))
        {
            //Digo que no está saltando ni cayendo (para que pueda volver a saltar)
            isJumping = false;
            isFalling = false;
            isGrounded = true;
           

            //Le quito la fuerza de salto remanente que tuviera
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            animator.SetBool("isJumping", false); //Si toca el suelo, acabo la animación
            animator.SetBool("isFalling", false); //Si toca el suelo, acabo la animación
        }
    }
}