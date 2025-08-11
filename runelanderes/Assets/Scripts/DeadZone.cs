using UnityEngine;

public class DeadZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void Ontrigger2D(other Collider2D)
   {
       Player_basicPlataform player = other.GetComponent<Player_basicPlataform>();
       if(player != null)
       {
           player.changeHealth(-1);
        }
    }
}
