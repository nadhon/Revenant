using UnityEngine;

public class TruqueMagicos : MonoBehaviour
{
    public GameObject projetil;
    public Transform pontoDeDisparo;

    public void CastarTruque()
    {
        if (projetil != null && pontoDeDisparo != null)
        {
          Instantiate(projetil, pontoDeDisparo.position, pontoDeDisparo.rotation);
        }
    }
}
