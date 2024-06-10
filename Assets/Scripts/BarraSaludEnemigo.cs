using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraSaludEnemigo : MonoBehaviour
{
    public GameObject[] vidas;

    public void DesactivarVida(int indice)
    {
        vidas[indice].SetActive(false);
    }
}
