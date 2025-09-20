using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagEnemy : MonoBehaviour
{
    [Header("Movimiento General")]
    [SerializeField] private float moveSpeed = 2f;      // Velocidad base de persecución
    [SerializeField] private Transform target;          // El protagonista a seguir

    [Header("ZigZag")]
    [SerializeField] private float zigzagAmplitude = 1f;  // Amplitud del zigzag (qué tanto se aleja de la línea recta)
    [SerializeField] private float zigzagFrequency = 2f;  // Frecuencia del zigzag (qué tan rápido oscila)

    private Vector3 startOffset;
    private float timeCounter;

    void Start()
    {
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        startOffset = transform.position;
    }

    void Update()
    {
        if (target == null) return;

        // Dirección hacia el jugador
        Vector3 direction = (target.position - transform.position).normalized;

        // Movimiento lineal hacia el jugador
        Vector3 move = direction * moveSpeed * Time.deltaTime;

        // Agregar movimiento en zigzag con seno
        timeCounter += Time.deltaTime * zigzagFrequency;
        Vector3 zigzagOffset = new Vector3(Mathf.Sin(timeCounter) * zigzagAmplitude, Mathf.Cos(timeCounter) * (zigzagAmplitude / 2), 0f);

        // Mover enemigo
        transform.position += move + zigzagOffset * Time.deltaTime;
    }
}