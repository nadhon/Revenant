using UnityEngine;

public class Progect : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall") || collision.CompareTag("Ground") || collision.CompareTag("Water"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    void Update()
    {
        
    }
}
