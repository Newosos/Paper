using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcupineEnemy : MonoBehaviour
{
    [Header("Parámetros de movimiento")]
    public float speed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 0.6f;

    [Header("Daño al jugador")]
    public int damage = 10;
    public float attackCooldown = 1f;

    [Header("Referencias")]
    public Transform player;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private float lastAttackTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();

            if (distanceToPlayer <= attackRange)
                TryAttackPlayer();
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (distanceToPlayer < attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

    }

    void ChasePlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            Flip();
    }

    void TryAttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

            if (hit != null)
            {
                PlayerHealth health = hit.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(damage);

                lastAttackTime = Time.time;
            }
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
