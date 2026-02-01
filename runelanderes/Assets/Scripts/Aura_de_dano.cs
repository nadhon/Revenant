using System;
using UnityEngine;

public class AuraDeDano : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void OnTriggerEnter2D(Collider2D other)
   {
       PlayerHealth player = other.GetComponent<PlayerHealth>();
       if(player != null)
       {
            player.TakeDamage(10);
        }
    }
}