using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraSaludEnemigo : MonoBehaviour
{
    public GameObject[] vidas;

    public void DesactivarVida(int damage)
    {
        for (int i = 0; i < damage && i < vidas.Length; i++)
        {
            if (vidas[i].activeSelf)
            {
                vidas[i].SetActive(false);
            }
        }
    }
}
