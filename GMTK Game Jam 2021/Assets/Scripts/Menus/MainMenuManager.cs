using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialLevel");
    }

    public void LevelSelect() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }

    public void Credits() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void MainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Options() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Options");
    }
}
