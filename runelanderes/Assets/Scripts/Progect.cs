using System;
using UnityEngine;

public class Progect : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float distanciaMaxima = 36f;
    private Vector2 direcao = Vector2.right;
    private Vector2 pontoInicial;
    private Rigidbody2D rb;
    private Animator anim;


    private bool isExploding = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pontoInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploding) return;

        transform.Translate(direcao * speed * Time.deltaTime);
        float distanciaPercorrida = Vector2.Distance(pontoInicial, transform.position);
        if (distanciaPercorrida >= distanciaMaxima)
        {
           Explodir();
        }
    }
    public void Explodir()
    {
        if (isExploding) return;

        isExploding = true;
        anim.SetBool("EXPLOSION", true);
        Destroy(gameObject, 0.5f); // Destroy after animation plays
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MoveEnemy enemy = collision.GetComponent<MoveEnemy>();
        if (isExploding) return;
        if (collision.CompareTag("enemy"))
        {
            bool isEnemy = collision.CompareTag("enemy");
            if (isEnemy)
            {
                
            }
            Explodir();
        }
        if (enemy != null)
        {
            enemy.Fix();
        }
        
        
    }


    
        internal void DefinirDirecao(Vector2 direcaoDisparo) => direcao = direcaoDisparo.normalized;
}
