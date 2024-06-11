using UnityEngine;

public class GravityPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Personaje>().EnableGravitySwitch();
            Destroy(gameObject); // Destruye el power-up después de ser recogido
        }
    }
}
