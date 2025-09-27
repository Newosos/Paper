using UnityEditor;
using UnityEngine;

public class ZigZagEnemy : MonoBehaviour
{
    [Header("Movimiento General")]
    [SerializeField] private float moveSpeed = 2f;      // Velocidad base de persecuci�n
    [SerializeField] private Transform target;          // El protagonista a seguir

    [Header("ZigZag")]
    [SerializeField] private float zigzagAmplitude = 1f;  // Amplitud del zigzag (qu� tanto se aleja de la l�nea recta)
    [SerializeField] private float zigzagFrequency = 2f;  // Frecuencia del zigzag (qu� tan r�pido oscila)

    [Header("Detecci�n")]
    [SerializeField] private float detectionRadius = 5f;  // Radio en el que el enemigo detecta al jugador
    private bool playerDetected = false;

    [Header("Reacciones")]
    [SerializeField] private float knockbackForce = 4f;   // Fuerza con la que es empujado al recibir golpe
    [SerializeField] private float attackPushback = 2f;   // Retroceso del jugador al ser golpeado
    private Animator animator;
    private Rigidbody2D rb;

    private Vector3 startOffset;
    private float timeCounter;

    void Start()
    {
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        startOffset = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target == null) return;

        //Agregar un area de detecci�n y el enemigo no te debe perseguir hasta que te detecte.
        // Esto puede ser con un circle o por un trigger de area
        if (!playerDetected && Vector2.Distance(transform.position, target.position) <= detectionRadius)
        {
            playerDetected = true; // Detecta UNA vez y ya no deja de seguirlo
        }

        if (!playerDetected) return; // Si todav�a no lo detecta, no hace nada

        // Direcci�n hacia el jugador
        Vector3 direction = (target.position - transform.position).normalized;

        // Movimiento lineal hacia el jugador
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        // Agregar movimiento en zigzag con seno
        timeCounter += Time.deltaTime * zigzagFrequency;
        Vector3 zigzagOffset = new Vector3(Mathf.Sin(timeCounter) * zigzagAmplitude, Mathf.Cos(timeCounter) * (zigzagAmplitude / 2), 0f);

        // Mover enemigo
        transform.position += move + zigzagOffset * Time.deltaTime;

        //Cuando lo golpees debe tener una reacci�n ya sea por animaci�n.
        // Con el transform o rigidbody lo empujar en x hacia el lado contrario y activas animacion
        //if(isFacingRight)
        //transform.position += new Vector3(4,) ESTO ES UN EJEMPLO, NO LO USES, HAZLO TU, PERO POR AHI VA...
        // --- C�DIGO PARA REACCI�N AL SER GOLPEADO ---
        // Llamaremos a esta funci�n desde el script del jugador
        // para que cuando lo golpee, reciba una fuerza de retroceso y muestre animaci�n.
        // Ejemplo de uso: enemy.TakeHit(Vector2.left);
        // (Vector2.left si el golpe viene desde la derecha)


        //Cuando golpea al jugador tambi�n debe tener una animaci�n o reacci�n.
        // Cuando llega a estar cerca de el jugador debe tener una animacion de ataque ya sea que solo se incline o si tenga mas movimiento. y al golpear, este debe retroceder un poco, se puede mover
        // con transforms o rigidbody y debe regresar a su animacion normal
        if (Vector2.Distance(transform.position, target.position) < 1.2f)
        {
            if (animator) animator.SetTrigger("Attack");

            // Empujar al jugador hacia atr�s al ser golpeado
            Rigidbody2D playerRb = target.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDir = (playerRb.position - (Vector2)transform.position).normalized;
                playerRb.AddForce(pushDir * attackPushback, ForceMode2D.Impulse);
            }
        }
    }

    // --- FUNCI�N PARA REACCI�N AL SER GOLPEADO ---
    public void TakeHit(Vector2 hitDirection)
    {
        if (animator) animator.SetTrigger("Hit");

        if (rb != null)
        {
            rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    // --- GIZMOS PARA VISUALIZAR EL RADIO DE DETECCI�N EN EL EDITOR ---
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}