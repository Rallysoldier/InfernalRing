using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour

      
{
    public GameObject mainMenu, settingsMenu;
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    public GameObject mainFirstButton, settingsFirstButton, settingsClosedButton;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    void Update()
    {
        
    }
        

    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        Debug.Log(volume);
    }
    public void SingleplayerGame()
    {
        SceneManager.LoadScene("StoryMode");
    }

    public void MultiplayerGame()
    {
        SceneManager.LoadScene("MapSelect");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);

        mainMenu.SetActive(false);

        //clears selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new selected object
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void CloseSettings()
    {
        mainMenu.SetActive(true);

        settingsMenu.SetActive(false);

        //clears selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new selected object
        EventSystem.current.SetSelectedGameObject(settingsClosedButton);
    }
    
}