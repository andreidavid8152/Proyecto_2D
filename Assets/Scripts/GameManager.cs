using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public HUD hud;
    public List<BarraSaludEnemigo> saludEnemigos = new List<BarraSaludEnemigo>();
    public int PuntosTotales { get { return puntosTotales; } }
    private int puntosTotales;
    private int vidas = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Mas de un game manager!");
        }
    }

    public void SumarPuntos(int puntosSumar)
    {
        puntosTotales += puntosSumar;
        hud.ActualizarPuntos(PuntosTotales);
    }

    public void PerderVida(int damage)
    {
        vidas -= damage;
        if (vidas <= 0)
        {
            SceneManager.LoadScene(2);
        }
        hud.DesactivarVida(vidas);
    }

    public void RegistrarEnemigo(BarraSaludEnemigo saludEnemigo)
    {
        saludEnemigos.Add(saludEnemigo);
    }

    public void EliminarEnemigo(BarraSaludEnemigo saludEnemigo)
    {
        saludEnemigos.Remove(saludEnemigo);
    }

    public void PerderVidaEnemigo(BarraSaludEnemigo saludEnemigo, int damage)
    {
        saludEnemigo.DesactivarVida(damage);
    }

    public bool RecuperarVida()
    {
        if (vidas == 3)
        {
            return false;
        }
        hud.ActivarVida(vidas);
        vidas += 1;
        return true;
    }
}
