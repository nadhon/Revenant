using System;
using System.Data.Common;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [Header("Configuração do inimigo")]
    [SerializeField] private Transform[] pontosDoCaminho;
    public float speed = 5f;
    private int pontoAtual = 0;

    [Header("Status")]

    public int Life = 90;
    public Rigidbody2D enemyRb;
    private bool indoParaFrente = true;
    private Animator Animator;
    [SerializeField] private float raioVision;
    [SerializeField] private int layerAreavisao;


    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        if (pontosDoCaminho.Length == 0)
        {
            Debug.LogError("Nenhum ponto de caminho definido para o inimigo.");
        }
        transform.position = pontosDoCaminho[pontoAtual].position;
    }

    // Update is called once per frame
    void Update()
    {
        Patrulhar();
    }
    private void Patrulhar()
    {
        if (pontosDoCaminho.Length == 0) return;
        Vector2 direcao = pontosDoCaminho[pontoAtual].position - transform.position;
        Vector2 novaPosicao = Vector2.MoveTowards(transform.position, pontosDoCaminho[pontoAtual].position, speed * Time.deltaTime);
        enemyRb.MovePosition(novaPosicao);

        if (Animator != null)
        {
            Animator.SetBool("Patrulha", true);
        }
        if (Vector2.Distance(transform.position, pontosDoCaminho[pontoAtual].position) < 0.1f)
        {
            if (indoParaFrente)
            {
                pontoAtual++;
                if (pontoAtual >= pontosDoCaminho.Length)
                {
                    pontoAtual = 0;
                }
                float direcaoX = pontosDoCaminho[pontoAtual].position.x - transform.position.x;
                if ((direcaoX < 0 && transform.localScale.x < 0) || (direcaoX > 0 && transform.localScale.x > 0))
                {
                    Virar();
                }
            }
        }
    }
    private void Virar()
    {
        Vector3 escalaAtual = transform.localScale;
        escalaAtual.x *= -1;
        transform.localScale = escalaAtual;
    }
    internal void TakeDamage(int damage)
    {
        Life -= damage;
        if (Life <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<LifebarPlayer>().TakeDamage(90);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (pontosDoCaminho != null && pontosDoCaminho.Length > 0)
        {
            Gizmos.color = Color.green;
            foreach (var ponto in pontosDoCaminho)
            {
                if (ponto != null)
                {
                    Gizmos.DrawSphere(ponto.position, 0.2f);
                }
            }
            for (int i = 0; i < pontosDoCaminho.Length; i++)
            {
                if (pontosDoCaminho[i] != null)
                {
                    Vector3 proximoPonto = pontosDoCaminho[(i + 1) % pontosDoCaminho.Length].position;
                    Gizmos.DrawLine(pontosDoCaminho[i].position, proximoPonto);
                }
            }
        }

        Gizmos.DrawWireSphere(transform.position, raioVision);
    }
}