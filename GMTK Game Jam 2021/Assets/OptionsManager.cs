using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject displayUIToggle;
    public GameObject soundVolumeSlider;
    public GameObject musicVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs are saved in PauseManager's ToggleOptionsMenuVisibility.
        if (!PlayerPrefs.HasKey("displayUI")) {
            PlayerPrefs.SetInt("displayUI", 1);
            PlayerPrefs.SetFloat("soundVolume", 1.0f);
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
            PlayerPrefs.Save();
        }
        Debug.Log(PlayerPrefs.GetInt("displayUI"));
        displayUIToggle.GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("displayUI") == 0;
        soundVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("soundVolume");
        musicVolumeSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void DisplayUIToggle(Toggle change) {
        PlayerPrefs.SetInt("displayUI", (change.isOn == false)? 1 : 0);
        if (GameObject.Find("PauseManager")) {
            GameObject.Find("PauseManager").GetComponent<PauseManager>().ToggleUIDisplay(!change.isOn);
        }
    }

    public void UpdateSoundVolume(Slider soundSlider) {
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
        if (GameObject.Find("PauseManager"))
        {
            GameObject.Find("PauseManager").GetComponent<PauseManager>().UpdateSFXVolume(soundSlider.value);
        }
    }

    public void UpdateMusicVolume(Slider musicSlider) {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        if (GameObject.Find("Music")) {
            GameObject.Find("Music").GetComponent<MusicManager>().UpdateVolume(musicSlider.value);
        }
    }

    public void SaveAndReturnToMainMenu() {
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
