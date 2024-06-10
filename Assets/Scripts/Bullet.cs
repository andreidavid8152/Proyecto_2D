using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;
    public int damage = 1; // Cantidad de daño que la bala inflige

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Rigidbody2D.velocity = Direction * speed;
    }

    public void setDirection(Vector2 direction)
    {
        Direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("EnemyBullet"))
        {
            Personaje personaje = other.GetComponent<Personaje>();
            personaje.SetDamage();
            GameManager.Instance.PerderVida(damage); // Aplicar daño al jugador
            Destroy(gameObject); // Destruir la bala al impactar
        }
        else if (other.CompareTag("EnemyBullet") && gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject); // Destruir la otra bala
            Destroy(gameObject); // Destruir esta bala
        }
        else if (other.CompareTag("PlayerBullet") && gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject); // Destruir la otra bala
            Destroy(gameObject); // Destruir esta bala
        }
        else if (other.CompareTag("Enemy") && gameObject.CompareTag("PlayerBullet"))
        {

            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                Debug.Log("dsaf");
                GameManager.Instance.PerderVidaEnemigo(damage); // Aplicar daño al jugador
                enemigo.RecibirDanio(damage); // Aplicar daño al enemigo
                Destroy(gameObject); // Destruir la bala al impactar
            }
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
