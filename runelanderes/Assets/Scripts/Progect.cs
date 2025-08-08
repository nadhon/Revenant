using System;
using UnityEngine;

public class Progect : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float distanciaMaxima = 18f;
    private Vector2 direcao = Vector2.right;
    private Vector2 pontoInicial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pontoInicial = transform.position;
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direcao * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy") || collision.CompareTag("Wall") || collision.CompareTag("ground") || collision.CompareTag("Water"))
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        float distanciaPercorrida = Vector2.Distance(pontoInicial, transform.position);
        if (distanciaPercorrida >= distanciaMaxima)
        {
            Destroy(gameObject);
        }
    }

    internal void DefinirDirecao(Vector2 direcaoDisparo) => direcao = direcaoDisparo.normalized;
}
