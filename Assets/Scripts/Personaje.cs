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
    public GameObject UltraShootPrefab;
    public AudioClip sonidoRecibirDanio;
    public bool isDead = false; // Agregar esta línea para la variable isDead
    private SpriteRenderer spriteRenderer;


    private bool canSwitchGravity = false;
    private bool gravityInverted = false;

    public ParticleSystem jumpEffect;
    public ParticleSystem runEffect;
    public ParticleSystem dropEffect1;
    public ParticleSystem dropEffect2;

    private int shoot2Count = 0;
    private float shoot2TimeWindow = 3.0f;
    private float shoot2Timer = 0.0f;

    private bool doubleShootEnabled = false;
    private int doubleShootCount = 0;
    private float doubleShootTimeWindow = 3.0f;
    private float doubleShootTimer = 0.0f;

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
        Physics2D.gravity = new Vector2(0, -9.8f); // Gravedad normal al inicio
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            movimiento();
            salto();
            // El resto de las acciones en Update...
        }

        // Llamar a Shoot1 cuando se presione el clic izquierdo del ratón
        if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del ratón
        {
            Shoot1();
        }

        // Lógica para accionar Shoot2 cuando se presione 3 veces la tecla H en un tiempo de 3s
        if (Input.GetKeyDown(KeyCode.H))
        {
            shoot2Count++;
            Debug.Log("Shoot2 count: " + shoot2Count);
            if (shoot2Count == 1)
            {
                shoot2Timer = shoot2TimeWindow;
            }
            else if (shoot2Count == 3)
            {
                Shoot2();
                shoot2Count = 0; // Reiniciar el contador
                shoot2Timer = 0; // Reiniciar el temporizador
            }
        }

        // Decrementar el temporizador para Shoot2 y resetear el contador si se acaba el tiempo
        if (shoot2Timer > 0)
        {
            shoot2Timer -= Time.deltaTime;
            if (shoot2Timer <= 0)
            {
                shoot2Count = 0; // Reiniciar el contador si el tiempo se agota
                Debug.Log("Shoot2 temporizador agotado, contador reiniciado.");
            }
        }

        // Lógica para accionar UltraShoot cuando se presione 4 veces el clic derecho del ratón en un tiempo de 3s
        // Dentro de Update, modifica la sección de UltraShoot
        if (Input.GetMouseButtonDown(1)) // 1 es el botón derecho del ratón
        {
            ultraShootCount++;
            Debug.Log("UltraShoot count: " + ultraShootCount);
            if (ultraShootCount == 1)
            {
                ultraShootTimer = ultraShootTimeWindow;
            }
            else if (ultraShootCount == 4)
            {
                UltraShoot();
                ultraShootCount = 0; // Reiniciar el contador
                ultraShootTimer = 0; // Reiniciar el temporizador
            }
        }

        // Decrementar el temporizador para UltraShoot y resetear el contador si se acaba el tiempo
        if (ultraShootTimer > 0)
        {
            ultraShootTimer -= Time.deltaTime;
            if (ultraShootTimer <= 0)
            {
                ultraShootCount = 0; // Reiniciar el contador si el tiempo se agota
                Debug.Log("UltraShoot temporizador agotado, contador reiniciado.");
            }
        }

        // Cambiar la gravedad al presionar una tecla, por ejemplo, la tecla G
        if (canSwitchGravity && Input.GetKeyDown(KeyCode.G))
        {
            SwitchGravity();
        }

        if (doubleShootEnabled && Input.GetKeyDown(KeyCode.M))
        {
            doubleShootCount++;
            Debug.Log("DoubleShoot count: " + doubleShootCount);
            if (doubleShootCount == 1)
            {
                doubleShootTimer = doubleShootTimeWindow;
            }
            else if (doubleShootCount == 2)
            {
                ActivateDoubleShoot();
                doubleShootCount = 0; // Reiniciar el contador
                doubleShootTimer = 0; // Reiniciar el temporizador
            }
        }

        // Decrementar el temporizador para DoubleShoot y resetear el contador si se acaba el tiempo
        if (doubleShootTimer > 0)
        {
            doubleShootTimer -= Time.deltaTime;
            if (doubleShootTimer <= 0)
            {
                doubleShootCount = 0; // Reiniciar el contador si el tiempo se agota
                Debug.Log("DoubleShoot temporizador agotado, contador reiniciado.");
            }
        }

    }

    public void EnableDoubleShoot()
    {
        doubleShootEnabled = true;
    }

    public void ActivateDoubleShoot()
    {
        StartCoroutine(DoubleShootCoroutine());
    }

    private IEnumerator DoubleShootCoroutine()
    {
        // Ejecutar Shoot1 y Shoot2 una sola vez
        Shoot1();
        Shoot2();
        yield return null; // Se detiene inmediatamente después de ejecutar los disparos
    }

    public void ChangeSize(bool isBig)
    {
        float scaleFactor = isBig ? 1.0f : 0.5f; // Factor de escala para tamaño grande o pequeño
                                                 // Ajustar la escala del personaje
        Vector3 newScale = new Vector3(-scaleFactor, -scaleFactor, transform.localScale.z);
        transform.localScale = newScale;
    }

    public void EnableGravitySwitch()
    {
        canSwitchGravity = true;
    }

    private void SwitchGravity()
    {
        gravityInverted = !gravityInverted;
        Physics2D.gravity = gravityInverted ? new Vector2(0, 9.8f) : new Vector2(0, -9.8f);

        // Invertir la orientación del personaje
        Vector3 theScale = transform.localScale;
        theScale.y *= -1;
        transform.localScale = theScale;

        // Encontrar todos los enemigos y cambiar su gravedad
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        foreach (Enemigo enemigo in enemigos)
        {
            Debug.Log("Cambio gravedad y orientación de los enemigos");
            enemigo.SwitchGravity();
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
            if (dropEffect1 != null && dropEffect2 != null)
            {
                dropEffect1.Play();
                dropEffect2.Play();
            }
        }
        enSuelo = sueloActual;
        if (Input.GetKeyDown(KeyCode.W) && saltosRestantes > 0)
        {
            if (dropEffect1 != null) dropEffect1.Stop();
            if (dropEffect2 != null) dropEffect2.Stop();
            if (runEffect != null) runEffect.Stop();
            jumpEffect.Play();
            saltosRestantes--;
            r.velocity = new Vector2(r.velocity.x, 0f);
            r.AddForce((gravityInverted ? Vector2.down : Vector2.up) * fuerzaSalto, ForceMode2D.Impulse); // Invertir dirección del salto si la gravedad está invertida
            AudioManager.Instance.ReproducirSonido(sonidoSalto);
        }
    }

    public void SetDamage()
    {
        AudioManager.Instance.ReproducirSonido(sonidoRecibirDanio);
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
        if (transform.localScale.x >= 0.5f) direction = Vector2.right;
        else direction = Vector2.left;

        Vector3 bulletPosition = transform.position + direction;
        bulletPosition.y -= 0.70f;

        bulletPosition.x -= 0.54f;

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

    // Función Shoot2 que establece isShoot2 en true y luego en false después de 0.45 segundos
    public void Shoot2()
    {
        animator.SetBool("isShoot2", true);
        Debug.Log("Parámetro 'isShoot2' se establecerá en true en el Animator del jugador.");
        StartCoroutine(ResetIsShoot2AndShootBullets(recoilDistanceShoot2));
    }

    private IEnumerator ResetIsShoot2AndShootBullets(float recoilDistance)
    {
        Debug.Log("Esperando para restablecer 'isShoot2'...");
        yield return new WaitForSeconds(0.45f);
        animator.SetBool("isShoot2", false);
        Debug.Log("Parámetro 'isShoot2' se ha restablecido a false en el Animator del jugador.");

        Vector3 direction;
        if (transform.localScale.x >= 0.5f) direction = Vector2.right;
        else direction = Vector2.left;

        // Ajustar la posición inicial más alta para ambas balas
        Vector3 bulletStartPosition = transform.position + direction;
        bulletStartPosition.y += 0.5f; // Aumentar la altura inicial de las balas

        // Posición de la primera bala
        Vector3 bulletPosition1 = bulletStartPosition;
        bulletPosition1.y -= 0.70f;
        GameObject bullet1 = Instantiate(Bullet1Prefab, bulletPosition1, Quaternion.identity);
        bullet1.tag = "PlayerBullet";
        bullet1.GetComponent<Bullet>().setDirection(direction);

        // Posición de la segunda bala (más junta)
        Vector3 bulletPosition2 = bulletStartPosition;
        bulletPosition2.y -= 0.40f; // Ajustar la altura de la segunda bala para que estén más juntas
        GameObject bullet2 = Instantiate(Bullet1Prefab, bulletPosition2, Quaternion.identity);
        bullet2.tag = "PlayerBullet";
        bullet2.GetComponent<Bullet>().setDirection(direction);

        // Aplica el recoil aquí después de disparar
        ApplyRecoil(recoilDistance);
    }

    // Función UltraShoot que establece isUltraShoot en true y luego en false después de 1.05 segundos
    public void UltraShoot()
    {
        animator.SetBool("isUltraShoot", true);
        Debug.Log("Parámetro 'isUltraShoot' se establecerá en true en el Animator del jugador.");
        StartCoroutine(ResetIsUltraShootAndShootBullets(recoilDistanceUltraShoot));
    }

    private IEnumerator ResetIsUltraShootAndShootBullets(float recoilDistance)
    {
        Debug.Log("Esperando para restablecer 'isUltraShoot'...");
        yield return new WaitForSeconds(1.05f); // Duración del efecto de UltraShoot
        animator.SetBool("isUltraShoot", false);
        Debug.Log("Parámetro 'isUltraShoot' se ha restablecido a false en el Animator del jugador.");

        Vector3 direction;
        if (transform.localScale.x >= 0.5f) direction = Vector2.right;
        else direction = Vector2.left;

        // Ajustar la posición inicial para la bala de ultra disparo
        Vector3 bulletPosition = transform.position + direction;
        bulletPosition.y -= 0.30f; // Ajustar según sea necesario

        // Instanciar el objeto UltraShoot
        GameObject bullet = Instantiate(UltraShootPrefab, bulletPosition, Quaternion.identity); // Asegúrate de tener un prefab para UltraShoot
        bullet.tag = "UltraBallet";
        bullet.GetComponent<ultrashoot>().setDirection(direction);

        // Aplica el recoil aquí después de disparar
        ApplyRecoil(recoilDistance);
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

    public void Die() // Nuevo método para manejar la muerte del personaje
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            StartCoroutine(DieCoroutine());
        }
    }


    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1.5f); // Espera 2 segundos para que se vea la animación de morir
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia el nivel
    }

    public void ChangeColor(Color newColor)
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashColor(newColor));
        }
    }

    private IEnumerator FlashColor(Color newColor)
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = newColor;
        yield return new WaitForSeconds(0.5f); // Duración del cambio de color
        spriteRenderer.color = originalColor;
    }

}


