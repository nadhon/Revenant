using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColletavadeVida : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerPlatformer player = collision.GetComponent<PlayerPlatformer>();

    if (player != null && player.VidaAtual < player.MaxVida)
      {
        player.ChangeHealth(1);
        Destroy(gameObject);
      }
  }

}
