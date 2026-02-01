using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float force;

    private Vector2 pontoInicial;

    

    public int damage = 0;

    [System.Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        rb.rotation = rot;
        pontoInicial = transform.position;
    }


    void Update()
    {
        float distanciaPercorrida = Vector2.Distance(pontoInicial, transform.position);
        if (distanciaPercorrida > 10f)
        {
            transform.localScale = new Vector3(0, 0, 0);
            Destroy(gameObject);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Enemy") && !collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}