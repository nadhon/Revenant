using UnityEngine;
using UnityEngine.UIElements;
public class PlayerHealth : MonoBehaviour
{
   public int MaxVida = 100;

   public int currentVida;

   private static UIHandler uiHandler;

   private void Awake()
    {
        currentVida = MaxVida;
        uiHandler = UIHandler.instance;

        if(uiHandler != null) uiHandler.SetHealthValue(1f);
    }
    public void TakeDamage(int damage)
    {
        currentVida -= damage;
        currentVida = Mathf.Clamp(currentVida, 0, MaxVida);

        if(uiHandler !=null)
        {
            float percentage = (float)currentVida / MaxVida;
            uiHandler.SetHealthValue(percentage);
        }
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
