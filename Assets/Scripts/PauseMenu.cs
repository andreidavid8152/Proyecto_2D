using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // UI del menú de pausa

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);  // Asegúrate de que el menú de pausa esté oculto al inicio
        Debug.Log("PauseMenu inicializado. Menú de pausa oculto.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Tecla Escape presionada.");
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resumiendo el juego.");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Reanudar el tiempo
        isPaused = false;
        AudioListener.pause = false;  // Reanudar el audio
    }

    void Pause()
    {
        Debug.Log("Pausando el juego.");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Pausar el tiempo
        isPaused = true;
        AudioListener.pause = true;  // Pausar el audio
    }

    public void LoadMenu()
    {
        Debug.Log("Cargando el menú principal.");
        Time.timeScale = 1f;  // Asegúrate de reanudar el tiempo antes de cargar el menú principal
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego.");
        Application.Quit();
    }

    public void TogglePause()
    {
        Debug.Log("Botón de pausa presionado.");
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Reiniciando el nivel actual.");
        Time.timeScale = 1f;  // Asegúrate de reanudar el tiempo antes de reiniciar el nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
