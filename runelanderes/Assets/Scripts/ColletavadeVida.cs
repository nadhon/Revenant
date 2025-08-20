using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColletavadeVida : MonoBehaviour
{
  public AudioClip collectedClip;

    public AudioClip CollectedClip { get; private set; }

    void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerPlatformer player = collision.GetComponent<PlayerPlatformer>();

    if (player != null && player.VidaAtual < player.MaxVida)
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
      {
        player.ChangeHealth(+1);
        Destroy(gameObject);
      }
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    {
      player.ChangeHealth(1);
      Destroy(gameObject);
    }
    player.PlayerSound(CollectedClip);
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
  }

}
