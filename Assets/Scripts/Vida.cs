using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    public AudioClip sonidoRecogerVida;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool vidaRecuperada = GameManager.Instance.RecuperarVida();
            if (vidaRecuperada)
            {
                AudioManager.Instance.ReproducirSonido(sonidoRecogerVida);
                other.gameObject.GetComponent<Personaje>().ChangeColor(new Color(1, 0.5f, 0.5f));
                Destroy(gameObject);
            }
        }
    }
}
