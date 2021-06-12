using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public LevelsList levelsList;
    public GameObject winMenu;
    public bool isPaused { 
        get {
            return paused;
        } 
    }
    public bool canUnpause = true;
    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu();
        }
    }

    public void PauseMenu() {
        PauseGame();
    }

    public void PauseGame() {
        paused = !paused;
        if (paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }
        else if (canUnpause) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1;
        }
    }

    public void RestartLevel() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GetWin() {
        PauseGame();
        canUnpause = false;
        winMenu.SetActive(true);
    }

    public void GetNextScene() {
        int curr_scene = 0;
        for (int i = 0; i < levelsList.levelsList.Length; i++) {
            if (levelsList.levelsList[i] == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) {
                curr_scene = i;
            }
        }
        TakeCount.currentScene = curr_scene + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelsList.levelsList[curr_scene + 1]);
    }
}
