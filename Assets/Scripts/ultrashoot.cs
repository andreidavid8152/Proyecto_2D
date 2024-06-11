using UnityEngine;

public class ultrashoot : MonoBehaviour
{
    public float speed;
    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;
    public int damage = 3; // Cantidad de daño que la bala inflige

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
        if (other.CompareTag("Enemy") && gameObject.CompareTag("UltraBallet"))
        {
            Enemigo enemigo = other.GetComponent<Enemigo>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(damage); // Aplicar daño al enemigo
                Destroy(gameObject); // Destruir la bala al impactar
            }
        }
        else if (other.CompareTag("EnemyBullet") && gameObject.CompareTag("UltraBallet"))
        {
            Debug.Log("chocando");
            Destroy(other.gameObject); // Destruir la otra bala
        }
    }

    public void DestroyUltra()
    {
        Destroy(gameObject);
    }
}
