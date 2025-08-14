using UnityEngine;

public class Patrulhadeinimigo : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody2d;
    public bool horizontal;
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        Animator animator;
        timer = changeTime;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (horizontal)
        {
            position.x = position.x + speed * direction * Time.deltaTime;
        }
        else
        {
            position.y = position.y + speed * direction * Time.deltaTime;
        }
        rigidbody2d.MovePosition(position);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPlatformer player = collision.gameObject.GetComponent<PlayerPlatformer>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
