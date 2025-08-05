using UnityEngine;

public class TruqueMagicos : MonoBehaviour
{
  [SerializeField] private Progect prefab;

  [SerializeField] private Transform firePoint;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      Shoot();
    }
  }

  private void Shoot()
  {
    if (prefab != null && firePoint != null)
    {
      Instantiate(prefab, firePoint.position, firePoint.rotation);
    }
  }

}
