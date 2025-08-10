using System;
using UnityEngine;
using UnityEngine.UI;

public class LifebarPlayer : MonoBehaviour
{
    public Slider healthSlider;

    [SerializeField] private GameObject player;

    public float Life { get; private set; }
    


    public void Initialize(float initialLife)
    {
        Life = initialLife;
        healthSlider.maxValue = Life;
        healthSlider.value = Life;
    }

    public void LowLifebar()
    {
        healthSlider.value = Life;
    }

    internal void TakeDamage(int damage)
    {
        Life -= damage;
        if (Life < 0) Life = 0;
        healthSlider.value = Life;

        if (Life <= 0)
        {
            // Handle death logic here, e.g., destroy the game object or trigger a death animation
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("DEATH", true);
            }
            Destroy(gameObject);
        }
    }
}
