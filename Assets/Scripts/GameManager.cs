using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public HUD hud;
    public List<BarraSaludEnemigo> saludEnemigos = new List<BarraSaludEnemigo>();
    public AudioClip sonidoPerder; // Agregar esta línea para el sonido de perder

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
            // Reproducir el sonido de perder
            AudioManager.Instance.ReproducirSonido(sonidoPerder);
            // Llamar al método Die en el personaje en lugar de reiniciar inmediatamente
            Personaje personaje = FindObjectOfType<Personaje>();
            if (personaje != null)
            {
                personaje.Die();
            }
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

    public void PerderVidaEnemigo(BarraSaludEnemigo saludEnemigo, int vidas)
    {
        Debug.Log(vidas);
        saludEnemigo.DesactivarVida(vidas);
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
