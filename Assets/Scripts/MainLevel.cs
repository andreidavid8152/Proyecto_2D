using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevel : MonoBehaviour
{
    public void Volver()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void Borrar()
    {

    }
}
