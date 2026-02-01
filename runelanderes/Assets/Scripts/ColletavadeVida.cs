using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColletavadeVida : MonoBehaviour
{
  public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D collision)
  {
    if (!collision.CompareTag("Player")) return;

    if (collision.TryGetComponent<PlayerHealth>(out var health))
    {
        health.TakeDamage(-20);
        
        AudioSource.PlayClipAtPoint(collectedClip, transform.position);
        Destroy(gameObject);
    }
  }
}