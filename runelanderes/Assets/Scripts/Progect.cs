using System;
using UnityEngine;

public class Progect : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float distanciaMaxima = 36f;
    [SerializeField] private int casterLevel = 1;
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
        if (isExploding) return;
        if (collision.CompareTag("enemy" ))
        {
            bool isEnemy = collision.CompareTag("enemy");
            if (isEnemy)
            {
                int damage = CalculateDamage();
                collision.GetComponent<LifebarPlayer>().TakeDamage(damage);
            }
            Explodir();
        }
        
    }

    private int CalculateDamage()
    {
        int dano = 1;
        if (casterLevel >= 17) dano = 4;
        else if (casterLevel >= 11) dano = 3;
        else if (casterLevel >= 5) dano = 2;

        int danoTotal = 0;
        for (int i = 0; i < dano; i++)
        {
            danoTotal += UnityEngine.Random.Range(1, 11); // Simula o lançamento de um dado de 10 faces
        }
        return danoTotal;
    }
        internal void DefinirDirecao(Vector2 direcaoDisparo) => direcao = direcaoDisparo.normalized;
}
