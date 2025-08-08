using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Video;

public class MoveEnemy : MonoBehaviour
{
    [Header("Configuração do inimigo")]
    [SerializeField] private Transform[] pontosDoCaminho;
    public float speed = 5f;
    private int pontoAtual = 0;

    [Header("Componetes")]

    public int vida = 90;
    public Rigidbody2D enemyRb;
    private bool indoParaFrente = true;
    private Animator Animator;
    private bool jogadorDetecado;

    [Header("Ataque")]
    [SerializeField] private Transform pontoDeAtaque;

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
        if (!jogadorDetecado)
        {
            Patrulhar();
        }
        Detectar();
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
    private void Detectar()
    {
        Vector2 direcao = transform.right * Mathf.Sign(transform.localScale.x);
        float distanciaDeteccao = 2f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcao, distanciaDeteccao, LayerMask.GetMask("Player"));
        Debug.DrawRay(transform.position, direcao * distanciaDeteccao, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            if (!jogadorDetecado)
            {
                jogadorDetecado = true;
                Animator.SetBool("Patrulha", false);
                Animator.SetTrigger("ATAQUE");

                Thunderwave thunder = GetComponent<Thunderwave>();
                if (thunder != null && pontoDeAtaque != null)
                {
                    thunder.transform.position = pontoDeAtaque.position;
                    thunder.Cast();
                }
            }
            else
            {
                jogadorDetecado = false;
                Animator.SetBool("Patrulha", true);
            }
            // Aqui você pode adicionar lógica para atacar o jogador
        }
    }
    internal void TakeDamage(int damage)
    {
        vida -= damage;
        if (vida <= 0)
        {
            Destroy(gameObject);
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
        if (pontoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pontoDeAtaque.position, 2f);
        }
    }
}