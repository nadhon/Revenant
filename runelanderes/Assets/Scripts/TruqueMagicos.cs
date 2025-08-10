using UnityEngine;

public class TruqueMagicos : MonoBehaviour
{
  [SerializeField] private Progect prefab;

  [SerializeField] private Transform firePoint;
  [SerializeField] private float timeBetweenShots = 0.5f;

  public PlayerInputActions PlayerInputActions { get; private set; }
  
  private float magicCounter = 0f;
  private object direcaoDisparo;

    private void Awake()
  {
    PlayerInputActions = new PlayerInputActions();
    if (prefab == null)
    {
      Debug.LogError("Prefab is not assigned in the TruqueMagicos script.");
    }
    if (firePoint == null)
    {
      Debug.LogError("Fire Point Transform is not assigned in the TruqueMagicos script.");
    }
    if (PlayerInputActions == null)
    {
      magicCounter -= Time.deltaTime;
      if (magicCounter <= 0f)
      {
        Instantiate(prefab, firePoint.position, firePoint.rotation);
        magicCounter = timeBetweenShots;
      }
    }
  }
  private void OnEnable()
  {
    PlayerInputActions.Player.Ataque.performed += ctx => Shoot();
    PlayerInputActions.Player.Enable();
  }
  private void Shoot()
  {
    if (prefab != null && firePoint != null)
    {
      var proj = Instantiate(prefab, firePoint.position, Quaternion.identity);
      Vector2 direcaoDisparo = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

      proj.DefinirDirecao(direcaoDisparo);
    }
  }


}
