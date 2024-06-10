using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public GameObject personaje;
    public GameObject bulletPrefab; // Prefab de la bala del enemigo
    public float shootingRange = 5.0f; // Distancia a la que el enemigo dispara
    public float cooldown = 3.0f; // Tiempo de cooldown entre disparos

    private float lastShotTime; // Tiempo del último disparo
    private Vector3 originalScale;

    private void Start()
    {
        // Guarda la escala original del enemigo al iniciar el juego
        originalScale = transform.localScale;
        lastShotTime = -cooldown; // Asegura que el enemigo pueda disparar inmediatamente al iniciar
    }

    private void Update()
    {
        Vector3 direction = personaje.transform.position - transform.position;
        if (direction.x >= 0.0f)
        {
            // Usa la escala original pero asegúrate de que esté mirando hacia la derecha
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            // Usa la escala original pero asegúrate de que esté mirando hacia la izquierda
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Verifica la distancia al personaje y dispara si está dentro del rango
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
        bullet.GetComponent<Bullet>().setDirection(direction);

        Debug.Log("Enemigo disparó una bala.");
    }
}
