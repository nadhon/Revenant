using System.Data.Common;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [Header("Configuração do inimigo")]
    [SerializeField] private Transform[] pontosDoCaminho;
    public float speed = 5f;
    private int pontoAtual = 0;

    [Header("Componetes")]
    public Rigidbody2D enemyRb;
    private bool indoParaFrente = true;
    private Animator enemyAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
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

        if (enemyAnimator != null)
        {
            enemyAnimator.SetBool("Patrulhando", true);
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
                if((direcaoX < 0 && transform.localScale.x > 0) || (direcaoX > 0 && transform.localScale.x < 0))
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
    
}