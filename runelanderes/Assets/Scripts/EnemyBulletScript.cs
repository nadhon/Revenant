using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D rb;
    public float force;

    private Vector2 pontoInicial;

    private Animator anim;

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
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        float distanciaPercorrida = Vector2.Distance(pontoInicial, transform.position);
        if (distanciaPercorrida > 10f)
        {
            anim.SetTrigger("Ataque");
        }
    }
}
