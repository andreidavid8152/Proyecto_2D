using UnityEngine;

public class DoorChecker : MonoBehaviour
{
    public int requiredPoints = 2;  // Puntos necesarios para abrir la puerta.
    public AudioClip successClip;  // Sonido de éxito.
    public AudioClip failureClip;  // Sonido de fallo.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int currentPoints = GameManager.Instance.PuntosTotales;
            if (currentPoints >= requiredPoints)
            {
                Personaje personaje = collision.GetComponent<Personaje>();
                if (personaje != null)
                {
                    personaje.SetSuccess();
                }
                else
                {
                    Debug.LogError("No se encontró un script Personaje en el objeto del jugador.");
                }
                AudioManager.Instance.ReproducirSonido(successClip);
            }
            else
            {
                AudioManager.Instance.ReproducirSonido(failureClip);
            }
        }
    }
}
