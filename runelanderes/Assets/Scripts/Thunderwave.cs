using UnityEngine;

public class Thunderwave : Spell
{
    public float radius = 5f;
    public int damage = 16;

    public float pushForce = 5f;
    
    public LayerMask targetLayer;
    private Rigidbody2D rb;

    public override void Cast()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var enemy = hit.GetComponent<MoveEnemy>();
                if (enemy != null) enemy.TakeDamage(damage);
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 pushDir = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}