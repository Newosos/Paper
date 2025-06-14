using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;


    [SerializeField] float movespeed = 3f;
    [SerializeField] float jumpSpeed = 3f;

    float keyHorizontal;
    bool keyJump;

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
        Debug.DrawRay(box2d_origin + new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box2d_origin - new Vector3(box2d.bounds.extents.x, 0), Vector2.down * (box2d.bounds.extents.y / 4f + raycastDistance), raycastColor);
        Debug.DrawRay(box2d_origin - new Vector3(box2d.bounds.extents.x, box2d.bounds.extents.y / 4f + raycastDistance), Vector2.right * (box2d.bounds.extents.x * 2), raycastColor);
    }

    //Update se da una vez por cuadro 

    void Update()
    {
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        keyJump = Input.GetKeyDown(KeyCode.Space);
        keyShoot = Input.GetKey(KeyCode.C);

        if(keyHorizontal < 0)
        {
            if (isFacingRigth)
            {
                Flip();
            }
            if(isGrounded)
            {
                animator.Play("Player_Run");
            }
          
            rb2d.velocity = new Vector2(-movespeed, rb2d.velocity.y);
        }
        else if (keyHorizontal >0)
        {
            if (isFacingRigth)
            {
                Flip();
            }
            if (isGrounded)
            {
                animator.Play("Player_Run");
            }

            animator.Play("Player_Run");
            rb2d.velocity = new Vector2(movespeed, rb2d.velocity.y);
        }
        else
        {
            if (isGrounded)
            {
                animator.Play("Player_idle");
             }
 
            
            rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
        }
       
        if (keyJump && isGrounded)
        {
            animator.Play("Player_Jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        }

        if(!isGrounded)
        {
            animator.Play("Player_Jump");
        }
            
    }
    void Flip()
    {
        isFacingRigth = !isFacingRigth;
        transform.Rotate(0f, 180f, 0f);
    }
}




