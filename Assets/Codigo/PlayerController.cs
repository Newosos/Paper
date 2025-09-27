using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;

    [Header("Movimiento")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] int maxJumps = 2;
    private int jumpCount;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 12f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 0.5f;
    private bool isDashing;
    private float lastDashTime;

    [Header("Ataques")]
    [SerializeField] Transform swordHitbox;
    [SerializeField] float swordRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] Transform distanceSlashPos;
    [SerializeField] GameObject distanceSlashPrefab;
    [SerializeField] float distanceSlashSpeed = 8f;

    [Header("Wall Jump")]
    [SerializeField] float wallSlideSpeed = 1.5f;
    [SerializeField] float wallJumpForce = 8f;
    [SerializeField] LayerMask wallLayer;
    private bool isTouchingWall;
    private bool isWallSliding;

    public bool isGrounded;
    private bool isFacingRight = true;

    float keyHorizontal;
    bool keyJump;
    bool keySlash;
    bool keyDistanceslash;
    bool keyDash;

    void Start()
    {
        animator = GetComponent<Animator>();
        box2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        keyHorizontal = Input.GetAxisRaw("Horizontal");
        keyJump = Input.GetButtonDown("Jump");
        keySlash = Input.GetKeyDown(KeyCode.C);
        keyDistanceslash = Input.GetKeyDown(KeyCode.V);
        keyDash = Input.GetKeyDown(KeyCode.LeftShift);

        GroundCheck();
        WallCheck();

        // --- MOVIMIENTO HORIZONTAL ---
        if (!isDashing)
            rb2d.velocity = new Vector2(keyHorizontal * moveSpeed, rb2d.velocity.y);

        if (keyHorizontal > 0 && !isFacingRight) Flip(); // 1
        else if (keyHorizontal < 0 && isFacingRight) Flip();

        if (isGrounded) // Este if revisa que estes en el suelo, para que te modifique el valor running, el cual es un parametro de animacion
        {
            // Necesitamos que este valor se este configurando constantemente porque no solo revisa si te mueves, sino tambien si no lo haces, para regresar al idle
            // puedes revisar tu animator para cersiorarte de esto
            animator.SetInteger("Running",(int)keyHorizontal);
        }
        // Animaciones de caminar / idle
        if (Mathf.Abs(keyHorizontal) > 0.1f && isGrounded)
        {
            if (keySlash)
                animator.Play("Player runslash");
        }
        else if (isGrounded)
        {
            animator.Play("Player idle");
        }

        // --- SALTO ---
        if (keyJump)
        {
            if (isGrounded)
            {
                Jump();
                jumpCount = 1;
            }
            else if (jumpCount < maxJumps)
            {
                Jump();
                jumpCount++;
            }
            else if (isWallSliding)
            {
                WallJump();
            }
        }

        // --- DASH ---
        if (keyDash && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        // --- ATAQUE MELEE ---
        if (keySlash && isGrounded && Mathf.Abs(keyHorizontal) < 0.1f)
        {
            animator.Play("Player slash");
            AttackMelee();
        }

        // --- ATAQUE A DISTANCIA ---
        if (keyDistanceslash)
        {
            animator.Play("Player slash"); // O crea "Player distanceslash" si la tienes
            ShootDistanceSlash();
        }

        // --- WALL SLIDE ---
        if (isTouchingWall && !isGrounded && keyHorizontal != 0)
        {
            isWallSliding = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            // Aquí podrías poner animación de pared si la tienes
        }
        else
        {
            isWallSliding = false;
        }

        // --- ANIMACIONES EN AIRE ---
        if (!isGrounded)
        {
            if (keySlash)
                animator.Play("Player jumpslash");
            else
                animator.Play("Player jump");
        }
    }

    void GroundCheck()
    {
        Vector2 origin = box2d.bounds.center;
        Vector2 size = box2d.bounds.size;
        isGrounded = Physics2D.OverlapBox(origin - new Vector2(0, 0.15f), new Vector2(size.x * 0.9f, 0.1f), 0f, LayerMask.GetMask("Ground"));
        // Aqui se le hizo una resta al origen para que el detector quede en los pies de el jugador
        if (isGrounded) jumpCount = 0;
    }

    void WallCheck()
    {
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        isTouchingWall = Physics2D.Raycast(transform.position, direction, 0.6f, wallLayer);
    }

    void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
        animator.Play("Player jump");
    }

    void WallJump()
    {
        Vector2 direction = isFacingRight ? Vector2.left : Vector2.right;
        rb2d.velocity = new Vector2(direction.x * moveSpeed, wallJumpForce);
        Flip();
        animator.Play("Player jump");
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;
        animator.Play("Player dash");

        float direction = isFacingRight ? 1f : -1f;
        rb2d.velocity = new Vector2(direction * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    void AttackMelee()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordHitbox.position, swordRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Golpeaste a " + enemy.name);
            enemy.GetComponent<ZigZagEnemy>()?.TakeHit((enemy.transform.position - transform.position).normalized);
        }
    }

    void ShootDistanceSlash()
    {
        GameObject slash = Instantiate(distanceSlashPrefab, distanceSlashPos.position, distanceSlashPos.rotation);
        Rigidbody2D slashRb = slash.GetComponent<Rigidbody2D>();
        float direction = isFacingRight ? 1f : -1f;
        slashRb.velocity = new Vector2(direction * distanceSlashSpeed, 0f);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    void OnDrawGizmosSelected()
    {
        if (swordHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(swordHitbox.position, swordRange);
        }
    }
}

