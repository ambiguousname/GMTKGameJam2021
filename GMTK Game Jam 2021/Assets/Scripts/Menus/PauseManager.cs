using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public LevelsList levelsList;
    public GameObject winMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject timerDisplay;
    public GameObject audienceEngagement;
    public GameObject levelManager;
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
        if (!PlayerPrefs.HasKey("displayUI"))
        {
            PlayerPrefs.SetInt("displayUI", 1);
            PlayerPrefs.SetFloat("soundVolume", 1.0f);
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
            PlayerPrefs.Save();
        }
        paused = false;
        ToggleUIDisplay(PlayerPrefs.GetInt("displayUI") == 1);
        UpdateSFXVolume(PlayerPrefs.GetFloat("soundVolume"));
        if (GameObject.Find("Music"))
        {
            var musicVolume = PlayerPrefs.GetFloat("musicVolume");
            GameObject.Find("Music").GetComponent<MusicManager>().UpdateVolume(musicVolume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseMenu();
        }
    }

    public void ToggleUIDisplay(bool onOff)
    {
        if (!onOff)
        {
            timerDisplay.SetActive(false);
            audienceEngagement.SetActive(false);
            levelManager.transform.GetChild(0).gameObject.SetActive(false);
            levelManager.transform.GetChild(1).gameObject.SetActive(false);
            if (levelManager.transform.GetChild(2)) {
                levelManager.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else {
            timerDisplay.SetActive(true);
            audienceEngagement.SetActive(true);
            levelManager.transform.GetChild(0).gameObject.SetActive(true);
            levelManager.transform.GetChild(1).gameObject.SetActive(true);
            if (levelManager.transform.GetChild(2)) {
                levelManager.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateSFXVolume(float volume) {
        pauseMenu.GetComponent<AudioSource>().volume = volume;
    }

    public void PauseMenu() {
        if (!(paused == true && pauseMenu.activeInHierarchy == false))
        {
            PauseGame();
            if (paused)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                if (optionsMenu.activeInHierarchy)
                {
                    ToggleOptionsMenuVisibility();
                }
                pauseMenu.SetActive(false);
            }
            var volume = PlayerPrefs.GetFloat("soundVolume");
            GetComponent<AudioSource>().volume = volume;
            GetComponent<AudioSource>().Play();
        }
    }

    public void TogglePauseMenuVisibility() {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
        }
        else {
            pauseMenu.SetActive(true);
        }
    }

    public void ToggleOptionsMenuVisibility() {
        if (optionsMenu.activeInHierarchy)
        {
            optionsMenu.SetActive(false);
            PlayerPrefs.Save();
        }
        else {
            optionsMenu.SetActive(true);
        }
    }

    public void MainMenu() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame() {
        paused = !paused;
        if (paused == false && canUnpause == false) {
            paused = false;
        }
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
        canUnpause = true;
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
        TakeCount.currentTake = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelsList.levelsList[curr_scene + 1]);
    }
}
