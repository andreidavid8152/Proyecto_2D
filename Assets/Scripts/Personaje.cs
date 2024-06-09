using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public int saltosMaximos;
    public LayerMask capaSuelo;
    public AudioClip sonidoSalto;

    private Rigidbody2D r;
    private BoxCollider2D b;
    private bool mirandoDerecha = true;
    private int saltosRestantes;
    private bool enSuelo;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        b = GetComponent<BoxCollider2D>();
        saltosRestantes = saltosMaximos;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movimiento();
        salto();
    }

    void movimiento()
    {
        float mov = Input.GetAxis("Horizontal");

        if (mov != 0f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        r.velocity = new Vector2(mov * velocidad, r.velocity.y);

        orientacion(mov);
    }

    void orientacion(float mov)
    {
        if ((mirandoDerecha == true && mov < 0) || (mirandoDerecha == false && mov > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    bool estaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(b.bounds.center, new Vector2(b.bounds.size.x, b.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider != null;
    }

    void salto()
    {
        bool sueloActual = estaEnSuelo();

        if (sueloActual && !enSuelo)
        {
            saltosRestantes = saltosMaximos;
        }

        enSuelo = sueloActual;

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            saltosRestantes--;
            r.velocity = new Vector2(r.velocity.x, 0f);
            r.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            AudioManager.Instance.ReproducirSonido(sonidoSalto);
        }
    }
}
