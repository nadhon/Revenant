using System;
using System.Data.Common;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [Header("Configuração do inimigo")]
    [SerializeField] private Transform[] pontosDoCaminho;
    public float speed = 5f;
    private int pontoAtual = 0;
    int direction = 1;

    [Header("Status")]

    public int Life = 90;
    public int currentLife;
    public Rigidbody2D enemyRb;
    private bool indoParaFrente = true;
    private Animator Animator;
    [SerializeField] private float raioVision;
    [SerializeField] private int layerAreavisao;
    public bool broken = true;


    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        if (pontosDoCaminho.Length == 0)
        {
            Debug.LogError("Nenhum ponto de caminho definido para o inimigo.");
        }
        currentLife = Life;
        transform.position = pontosDoCaminho[pontoAtual].position;
    }

    // Update is called once per frame
    void Update()
    {
        Patrulhar();
    }

    void FixedUpdate()
    {
        Animator.SetFloat("Blend", direction);
        Animator.SetFloat("Blend", 0);
        if (!broken)
        {
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPlatformer player = collision.gameObject.GetComponent<PlayerPlatformer>();
        if (player != null)
        {
            player.ChangeHealth(-1);

            Thunderwave();
        }
    }

    private void Thunderwave()
    {
        Debug.Log("Thunderwave");
    }

    public void Fix()
    {
        broken = false;

        GetComponent<Rigidbody2D>().simulated = false;
        Animator.SetTrigger("DEATH");
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
    
    public void ChangeLife(int amount)
    {
        currentLife = Math.Clamp(currentLife + amount, 0, Life);
        if (amount < 0)
        {
            if (currentLife <= 0)
            {
                Animator.SetBool("DEATH", true);
                Destroy(gameObject);
            }
        }
    }
}