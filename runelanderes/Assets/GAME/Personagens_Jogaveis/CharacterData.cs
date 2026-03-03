using UnityEngine;

[CreateAssetMenu(menuName = "Character/Data")]
public class CharacterData : ScriptableObject
{
    public float moveSpeed;
    public float jumpForce;
    public int maxHealth;
}
