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
        Personaje personaje = other.GetComponent<Personaje>();
        if (other.gameObject.CompareTag("Player"))
        {
            personaje.SetDamage();
            GameManager.Instance.PerderVida(damage); // Aplicar daño al jugador
            Destroy(gameObject); // Destruir la bala al impactar
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
