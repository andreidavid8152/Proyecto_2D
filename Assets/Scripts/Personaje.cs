using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public int saltosMaximos;
    public LayerMask capaSuelo;
    public AudioClip sonidoSalto;
    public GameObject Bullet1Prefab;

    public ParticleSystem jumpEffect;
    public ParticleSystem runEffect;
    public ParticleSystem dropEffect1;
    public ParticleSystem dropEffect2;

    private int shoot2Count = 0;
    private float shoot2TimeWindow = 3.0f;
    private float shoot2Timer = 0.0f;

    private int ultraShootCount = 0;
    private float ultraShootTimeWindow = 3.0f;
    private float ultraShootTimer = 0.0f;

    public float recoilDistanceShoot1 = 0.5f; // Distancia de retroceso para Shoot1
    public float recoilDistanceShoot2 = 1.0f; // Distancia de retroceso para Shoot2
    public float recoilDistanceUltraShoot = 1.5f; // Distancia de retroceso para UltraShoot

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

        // Llamar a Shoot1 cuando se presione el clic izquierdo del ratón
        if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del ratón
        {
            Shoot1();
        }

        // Lógica para accionar Shoot2 cuando se presione 3 veces la tecla H en un tiempo de 3s
        if (Input.GetKeyDown(KeyCode.H))
        {
            shoot2Count++;
            if (shoot2Count == 1)
            {
                shoot2Timer = shoot2TimeWindow;
                StartCoroutine(Shoot2Timer());
            }
            else if (shoot2Count == 3)
            {
                Shoot2();
                shoot2Count = 0; // Reiniciar el contador
            }
        }

        // Decrementar el temporizador para Shoot2
        if (shoot2Timer > 0)
        {
            shoot2Timer -= Time.deltaTime;
            if (shoot2Timer <= 0)
            {
                shoot2Count = 0; // Reiniciar el contador si el tiempo se agota
            }
        }

        // Lógica para accionar UltraShoot cuando se presione 4 veces el clic derecho del ratón en un tiempo de 3s
        if (Input.GetMouseButtonDown(1)) // 1 es el botón derecho del ratón
        {
            ultraShootCount++;
            if (ultraShootCount == 1)
            {
                ultraShootTimer = ultraShootTimeWindow;
                StartCoroutine(UltraShootTimer());
            }
            else if (ultraShootCount == 4)
            {
                UltraShoot();
                ultraShootCount = 0; // Reiniciar el contador
            }
        }

        // Decrementar el temporizador para UltraShoot
        if (ultraShootTimer > 0)
        {
            ultraShootTimer -= Time.deltaTime;
            if (ultraShootTimer <= 0)
            {
                ultraShootCount = 0; // Reiniciar el contador si el tiempo se agota
            }
        }
    }

    void movimiento()
    {
        float mov = Input.GetAxis("Horizontal");
        bool estaCorriendo = mov != 0f && enSuelo;

        if (estaCorriendo)
        {
            if (!animator.GetBool("isRunning"))
            {
                Debug.Log("Iniciando correr y efectos de partículas.");
                animator.SetBool("isRunning", true);
                runEffect.Play();

                if (dropEffect1 != null)
                {
                    dropEffect1.Stop();
                }
                if (dropEffect2 != null)
                {
                    dropEffect2.Stop();
                }
            }
        }
        else
        {
            if (animator.GetBool("isRunning"))
            {
                Debug.Log("Deteniendo correr y efectos de partículas.");
                animator.SetBool("isRunning", false);
                runEffect.Stop();
            }
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

            // Activa el efecto de partículas cuando aterriza
            if (dropEffect1 != null && dropEffect2 != null)
            {
                dropEffect1.Play();
                dropEffect2.Play();
            }
        }

        enSuelo = sueloActual;

        if (Input.GetKeyDown(KeyCode.W) && saltosRestantes > 0)
        {
            // Detener los efectos de partículas antes de saltar
            if (dropEffect1 != null)
            {
                dropEffect1.Stop();
            }
            if (dropEffect2 != null)
            {
                dropEffect2.Stop();
            }
            if (runEffect != null)
            {
                runEffect.Stop();
            }

            jumpEffect.Play();
            saltosRestantes--;
            r.velocity = new Vector2(r.velocity.x, 0f);
            r.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            AudioManager.Instance.ReproducirSonido(sonidoSalto);
        }
    }

    public void SetDamage()
    {
        animator.SetBool("isDamage", true);
        Debug.Log("Parámetro 'isDamage' se establecerá en true en el Animator del jugador.");
        StartCoroutine(ResetIsDamage());
    }

    private IEnumerator ResetIsDamage()
    {
        Debug.Log("Esperando para restablecer 'isDamage'...");
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("isDamage", false);
        Debug.Log("Parámetro 'isDamage' se ha restablecido a false en el Animator del jugador.");
    }

    // Función Shoot1 que establece isShoot1 en true y luego en false después de 0.42 segundos
    public void Shoot1()
    {
        animator.SetBool("isShoot1", true);
        StartCoroutine(ResetIsShoot1AndShootBullet(recoilDistanceShoot1));
    }

    private IEnumerator ResetIsShoot1AndShootBullet(float recoilDistance)
    {
        yield return new WaitForSeconds(0.42f);
        animator.SetBool("isShoot1", false);

        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector2.right;
        else direction = Vector2.left;

        Vector3 bulletPosition = transform.position + direction;
        bulletPosition.y -= 0.70f;

        GameObject bullet = Instantiate(Bullet1Prefab, bulletPosition, Quaternion.identity);
        bullet.tag = "PlayerBullet";
        bullet.GetComponent<Bullet>().setDirection(direction);

        // Aplica el recoil aquí después de disparar
        ApplyRecoil(recoilDistance);
    }

    private void ApplyRecoil(float recoilDistance)
    {
        Vector3 recoilDirection = new Vector3(-transform.localScale.x * recoilDistance, 0, 0);
        transform.position += recoilDirection; // Aplica el recoil moviendo la posición del personaje
    }


    private IEnumerator Shoot2Timer()
    {
        yield return new WaitForSeconds(shoot2TimeWindow);
        shoot2Count = 0;
    }

    // Función Shoot2 que establece isShoot2 en true y luego en false después de 0.45 segundos
    public void Shoot2()
    {
        animator.SetBool("isShoot2", true);
        Debug.Log("Parámetro 'isShoot2' se establecerá en true en el Animator del jugador.");
        StartCoroutine(ResetIsShoot2());
    }

    private IEnumerator ResetIsShoot2()
    {
        Debug.Log("Esperando para restablecer 'isShoot2'...");
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("isShoot2", false);
        Debug.Log("Parámetro 'isShoot2' se ha restablecido a false en el Animator del jugador.");
    }

    private IEnumerator UltraShootTimer()
    {
        yield return new WaitForSeconds(ultraShootTimeWindow);
        ultraShootCount = 0;
    }

    // Función UltraShoot que establece isUltraShoot en true y luego en false después de 1.05 segundos
    public void UltraShoot()
    {
        animator.SetBool("isUltraShoot", true);
        Debug.Log("Parámetro 'isUltraShoot' se establecerá en true en el Animator del jugador.");
        StartCoroutine(ResetIsUltraShoot());
    }

    private IEnumerator ResetIsUltraShoot()
    {
        Debug.Log("Esperando para restablecer 'isUltraShoot'...");
        yield return new WaitForSeconds(1.05f);
        animator.SetBool("isUltraShoot", false);
        Debug.Log("Parámetro 'isUltraShoot' se ha restablecido a false en el Animator del jugador.");
    }

    public void SetSuccess()
    {
        animator.SetBool("isSuccess", true);
        Debug.Log("Parámetro 'isSuccess' se establecerá en true en el Animator del jugador.");
        StartCoroutine(ResetIsSuccess());
    }

    private IEnumerator ResetIsSuccess()
    {
        Debug.Log("Esperando para restablecer 'isSuccess'...");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }

}


