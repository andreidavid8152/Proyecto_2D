using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLevel : MonoBehaviour
{
    public Button level2Button;  // Asignar el botón de nivel 2 desde el Inspector
    public GameObject lockButton;  // Asignar el objeto de bloqueo del botón de nivel 2 desde el Inspector
    public Button deleteGameButton;  // Asignar el botón de borrar progreso desde el Inspector

    void Start()
    {
        UpdateLevelButtons();
    }

    public void Volver()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    // Método para completar el nivel y guardar el progreso
    public void CompleteLevel(int levelIndex)
    {
        Debug.Log("Nivel " + levelIndex + " completado.");
        PlayerPrefs.SetInt("Level" + levelIndex + "Completed", 1);
        PlayerPrefs.Save();
        UpdateLevelButtons();  // Actualizar el estado de los botones de nivel
    }

    // Método para borrar el progreso del juego
    public void DeleteProgress()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Progreso del juego borrado.");
        UpdateLevelButtons();  // Actualizar el estado de los botones de nivel
    }

    // Método para actualizar el estado de los botones de nivel
    private void UpdateLevelButtons()
    {
        // Comprobar si el nivel 1 está completado
        if (PlayerPrefs.GetInt("Level1Completed") == 1)
        {
            level2Button.interactable = true;
            lockButton.SetActive(false);  // Ocultar el bloqueo del botón de nivel 2
        }
        else
        {
            level2Button.interactable = false;
            lockButton.SetActive(true);  // Mostrar el bloqueo del botón de nivel 2
        }
    }
}
