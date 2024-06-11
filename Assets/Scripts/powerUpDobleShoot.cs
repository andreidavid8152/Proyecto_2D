using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpDobleShoot : MonoBehaviour
{
    public AudioClip sonidoPowerUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.ReproducirSonido(sonidoPowerUp);
            Personaje personaje = collision.GetComponent<Personaje>();
            if (personaje != null)
            {
                personaje.EnableDoubleShoot(); // Permite la activación del doble disparo
                Destroy(gameObject); // Destruir el power-up después de recogerlo
            }
        }
    }
}
