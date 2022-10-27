using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour
{
  //  public GameObject pauseMenu, optionsMenu;
    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;

    //public GameObject pauseFirstButton, optionsFirstButton, optionsClosedButton;

    void Start()
    {

        Debug.Log("in_char_select");
        
    }
}