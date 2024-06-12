using UnityEngine;

public class GravityPowerUp : MonoBehaviour
{
    public AudioClip sonidoPowerUp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.ReproducirSonido(sonidoPowerUp);
            collision.gameObject.GetComponent<Personaje>().EnableGravitySwitch();
            collision.gameObject.GetComponent<Personaje>().ChangeColor(Color.gray);
            Destroy(gameObject); // Destruye el power-up después de ser recogido
        }
    }
}
