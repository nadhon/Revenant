using UnityEngine;

[CreateAssetMenu(fileName = "NovaClasse", menuName = "Classes/Nova Classe")]
public abstract class ClasseData : ScriptableObject
{
    public string classeName;

    public Sprite icon;

    public float cooldown;

    public abstract void Use(GameObject user);
}