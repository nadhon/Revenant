using System.Collection;
using System.Collection.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColletavadeVida : MonoBeHaviour
{
  void OntriggerEnter2D(Collider2D other)
  {
    Player_BasicPlataform player = other.GetComponet<Player_BasicPlataform>();

    if (player != null && player.Health<player.Maxheath)
      {
        player.changeHealth(1);
        Destroy(gameObject);
      }
  }
  
}
