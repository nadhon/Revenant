using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void OnTriggerEnter2D(Collider2D other)
   {
       PlayerHealth health = other.GetComponent<PlayerHealth>();
       if (health != null)
       {
           health.kill();
       }
    }
}