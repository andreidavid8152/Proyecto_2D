using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemigo : MonoBehaviour
{
    public GameObject personaje;
    public GameObject bulletPrefab;
    public float shootingRange = 5.0f;
    public float cooldown = 3.0f;
    public int maxVidas = 3;
    private int vidas;
    private float lastShotTime;
    private Vector3 originalScale;
    public BarraSaludEnemigo barraSalud; // Asigna la barra de salud en el inspector
    public AudioClip sonidoRecibirDanio;
    public AudioClip explosionSonido;
    public GameObject efectoExplosion;

    private bool gravityInverted = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        originalScale = transform.localScale;
        lastShotTime = -cooldown;
        vidas = maxVidas;
        GameManager.Instance.RegistrarEnemigo(barraSalud); // Registra el enemigo en el GameManager
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchGravity()
    {
        gravityInverted = !gravityInverted;

        // Invertir la orientación del sprite del enemigo
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.flipY = !spriteRenderer.flipY;
        }
    }

    private void Update()
    {
        Vector3 direction = personaje.transform.position - transform.position;
        if (direction.x >= 0.0f)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        if (Vector3.Distance(personaje.transform.position, transform.position) <= shootingRange)
        {
            if (Time.time >= lastShotTime + cooldown)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }

    private void Shoot()
    {
        Vector3 direction = personaje.transform.position - transform.position;
        direction.Normalize();
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.tag = "EnemyBullet";
        bullet.GetComponent<Bullet>().setDirection(direction);
        Debug.Log("Enemigo disparó una bala.");
    }

    public void RecibirDanio(int damage)
    {
        AudioManager.Instance.ReproducirSonido(sonidoRecibirDanio);
        vidas -= damage;
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        Debug.Log(vidas);
        if (vidas < 0)
        {
            vidas = 0; ;
        }
        GameManager.Instance.PerderVidaEnemigo(barraSalud, vidas);
        if (vidas <= 0)
        {
            explosion();
            GameManager.Instance.EliminarEnemigo(barraSalud);
            Destroy(gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f); // Duración del cambio de color
        spriteRenderer.color = Color.white;
    }

    private void OnDestroy()
    {
        GameManager.Instance.EliminarEnemigo(barraSalud); // Elimina el enemigo del GameManager al ser destruido
    }

    public void explosion()
    {
        Instantiate(efectoExplosion, transform.position, Quaternion.identity);
        AudioManager.Instance.ReproducirSonido(explosionSonido);
        cinemachineMovimientoCamara.Instance.MoverCamara(8, 5, 0.8f);
    }

}
