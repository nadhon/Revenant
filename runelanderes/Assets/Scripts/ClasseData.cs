using UnityEngine;

[CreateAssetMenu(fileName = "NovaClasse", menuName = "Classes/Nova Classe")]
public class ClasseData : ScriptableObject
{
    public ClassBase classe;
    [Header("Atributos")]
    public float vidaMaxima = 100f;

    public float danoBase = 10f;
    public float moveSpeed = 5f;

    [Header("Habilidades")]
    public GameObject atackPrefab;

    public GameObject truquePrefab;

    [Header("Aparencia")]

    public RuntimeAnimatorController animacao;

    public Sprite sprite;
}