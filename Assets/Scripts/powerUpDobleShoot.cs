using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpDobleShoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Personaje personaje = collision.GetComponent<Personaje>();
            if (personaje != null)
            {
                personaje.EnableDoubleShoot(); // Permite la activación del doble disparo
                Destroy(gameObject); // Destruir el power-up después de recogerlo
            }
        }
    }
}
