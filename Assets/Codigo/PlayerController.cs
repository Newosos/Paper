using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;

    Vector3 box2d_origin; // x,y,z // 0,0,0
    
    private bool isGrounded;
    private bool isFacingRigth;
    [SerializeField] float movespeed = 3f;
    [SerializeField] float jumpSpeed = 3f;


    float keyHorizontal;
    bool keyJump;
    bool keySlash;
    //bool keyDash;
    bool isSlashing;
    
    //start se llama antes del primer frame update

    void Start()
    {
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        //El sprite mira a la derecha por default
        isFacingRigth = true;
    }
    
    
    private void FixedUpdate()
    {
        isGrounded = false;
        Color raycastColor;
        RaycastHit2D raycastHit;
        float raycastDistance = 0.05f;
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        
        //Ground Check
        Vector2 box_origin = box2d.bounds.center;
        box_origin.y = box2d.bounds.min.y + (box2d.bounds.extents.y / 4f);
        Vector2 box_size = box2d.bounds.size;
        box_size.y = box2d.bounds.size.y / 4f;
        raycastHit = Physics2D.BoxCast(box_origin, box_size, 0f, Vector2.down, raycastDistance, layerMask);
        // player boxcolliding with ground layer
        if (raycastHit.collider != null)
        {
            isGrounded = true;
        }

        // draw debug lines

        raycastColor = (isGrounded) ? Color.green : Color.red;
        Debug.DrawRay(box_origin + new Vector2(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector2(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box_origin - new Vector2(box2d.bounds.extents.x, box2d.bounds.extents.y / 4f + raycastDistance), Vector2.right * (box2d.bounds.extents.x * 2), raycastColor);
    }

    //Update se da una vez por cuadro 

    void Update() // Se ejecuta cada frame
    {
        keyHorizontal = Input.GetAxisRaw("Horizontal"); // keyHorizontal esta revisando si estas presionando la A o la D, y segun la que presiones te da un 1 o un -1
        keyJump = Input.GetKeyDown(KeyCode.Space);
       // keyDash = Input.GetKeyDown(KeyCode.Q);
        keySlash = Input.GetKeyDown(KeyCode.C); // keySlash es verdadero si se presiona la tecla C
        isSlashing = keySlash;

        

        if(keyHorizontal < 0) // Si se presiona la tecla izquierda A o la flecha izquierda
        { // keyHorizontal = -1
            if (isFacingRigth)
            {
                Flip();
            }
            if(isGrounded) // Y si estas en el suelo
            {
                
                
                if (isSlashing) // Y si estas atacando
                {
                    animator.Play("Player runslash");
                }
                
            }
          
            rb2d.velocity = new Vector2(-movespeed, rb2d.velocity.y);
        }
        else if (keyHorizontal > 0) // Si estas caminando a la derecha, presionando la tecla D o la flecha derecha
        { // keyHorizontal = 1
            if (!isFacingRigth)
            {
                Flip();
            }
            if (isGrounded) // Y estas en el suelo
            {

                if (isSlashing) // Y si estas atacando
                {
                    animator.Play("Player runslash");
                }
               

            }
                        
            rb2d.velocity = new Vector2(movespeed, rb2d.velocity.y);
        }
        else
        {
            if (isGrounded) // Si estas en el suelo
            {
                if (isSlashing) // Y presionas la tecla C
                {
                    animator.Play("Player slash"); // Animacion de ataque
                    Debug.Log("Slash");
                }
            }
 
            
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }
        
        
        if(isGrounded) // Si estoy en el suelo
            animator.SetInteger("Running",(int)keyHorizontal); // Animacion de correr

        if (keyJump && isGrounded) // Si presionas la tecla de salto y estas en el suelo
        {
            if (isSlashing) // Si esta atacando
            {
                animator.Play("Player jumpslash"); // Animacion de ataque en el aire
            }
            else // Si no
            {
                animator.Play("Player jump"); // Animacion de ataque normal
            }

            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed); // Esta linea te mueve la velocidad en y segun el jumpSpeed
        }

        if (!isGrounded) // Si no estas en el suelo
        {
            if (isSlashing)
            {
                animator.Play("Player jumpslash");
            }
            else
            {
              animator.Play("Player jump");

            }
        }

    }
    void Flip()
    {
        isFacingRigth = !isFacingRigth;
        transform.Rotate(0f, 180f, 0f);
    }
}