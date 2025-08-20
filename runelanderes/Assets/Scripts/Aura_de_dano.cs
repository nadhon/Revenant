using System;
using UnityEngine;

public class AuraDeDano : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void OnTriggerEnter2D(Collider2D other)
   {
       PlayerPlatformer player = other.GetComponent<PlayerPlatformer>();
       if(player != null)
       {
            player.ChangeHealth(-10);// Resets player's health to zero
        }
    }
}