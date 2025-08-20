using UnityEngine;

public class Thunderwave : Spell
{
    [Header("Configurações da Magia")]
    public float radius = 5f;
    public int damage = 16;

    public float pushForce = 5f;

    public LayerMask targetLayer;
    [Header("Efeitos")]
    public GameObject thunderwaveVFX;
    public AudioClip thunderSFX;
    public float sfxVolume = 0.8f;

    public override void Cast()
    {
        if (thunderwaveVFX != null)
        {
            GameObject vfx = Instantiate(thunderwaveVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }
        if (thunderSFX != null)
        {
            AudioSource.PlayClipAtPoint(thunderSFX, transform.position, sfxVolume);
        }
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
                {
                    var player = hit.GetComponent<PlayerPlatformer>();
                    if (player != null)
                    {
                        player.ChangeHealth(damage);
                    }
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