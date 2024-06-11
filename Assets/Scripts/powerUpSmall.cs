using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpSmall : MonoBehaviour
{
    public AudioClip sonidoPowerUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.ReproducirSonido(sonidoPowerUp);
            collision.gameObject.GetComponent<Personaje>().ChangeSize(false);
            Destroy(gameObject); // Destruye el power-up después de ser recogido
        }
    }
}
