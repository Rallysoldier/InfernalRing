using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Character_Select_Screen_Controller : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            Application.LoadLevel("GameTest");
        }        
    }

    public void ChangeScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
