using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
   public int MaxVida = 100;

   public int currentVida;

   private void Awake()
    {
        currentVida = MaxVida;
    }
    public void TakeDamage(int damage)
    {
        currentVida -= damage;
        currentVida = Mathf.Clamp(currentVida, 0, MaxVida);
        if (currentVida <= 0)
        {
            kill();
        }
    }
    public void kill()
    {
        currentVida = 0;
    }
}
